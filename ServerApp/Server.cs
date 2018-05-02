using DataLayer;
using DataLayer.Data;
using ServerApp.TerminalServices;
using ServerApp.TerminalServices.Inform;
using ServerApp.TerminalServices.Order;
using ServerApp.TerminalServices.Serve;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ServerApp
{
	class Server
	{
		// hlavna cast programu ktora spusta jednotlive obsluhy
		DatabaseLayer dbLayer = new DatabaseLayer();
		public void Start()
		{
			foreach (MyApplication subApp in dbLayer.GetSubApps())
			{
				if (subApp.IsRunning)
				{
					Console.WriteLine($"Starting: {subApp.TypeName}");
					StartVirtualDevices(subApp.Devices);
					// podla typu aplikacie sa vytvori nove vlakno a zavola potrebna obsluha
					switch (subApp.TypeName.TrimEnd(' '))
					{
						case "Inform":
							Thread t1 = new Thread(new ParameterizedThreadStart(RunSubApp));
							t1.Start(new InformTerminalService(subApp.Devices.ToList(), subApp, dbLayer));
							break;
						case "Objed":
							Thread t2 = new Thread(new ParameterizedThreadStart(RunSubApp));
							t2.Start(new OrderTerminalService(subApp.Devices.ToList(), subApp, dbLayer));
							break;
						case "Vydaj":
							Thread t3 = new Thread(new ParameterizedThreadStart(RunSubApp));
							t3.Start(new ServeTerminalService(subApp.Devices.ToList(), subApp, dbLayer));
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
				else
				{

					Console.WriteLine($"OFF: {subApp.TypeName}");
				}
			}
			Console.WriteLine("All apps started");
		}

		// v pripade ze ip adresa je nasho PC, spusti emulator terminalu
		private void StartVirtualDevices(ICollection<Device> devices)
		{
			foreach (Device device in devices)
			{
				if (device.IP == "127.0.0.1")
					Process.Start(@"C:\p\ctecky\Trunk\Src\Emulators\QtEmulator\QTTcpServer.exe", $"-e -p {device.Port}");
				//Process.Start(@"D:\git\build-Rallo-Desktop_Qt_5_10_0_MinGW_32bit-Release\release\RalloApp.exe", $"-e -p {device.Port}");
			}
		}

		// funkcia na spustenie obsluhy na novom vlakne
		private void RunSubApp(object o)
		{
			TerminalService subApp = o as TerminalService;
			subApp.Start();
		}
	}
}
