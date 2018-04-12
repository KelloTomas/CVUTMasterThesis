using DataLayer;
using DataLayer.Data;
using ServerApp.SubApps;
using ServerApp.SubApps.Inform;
using ServerApp.SubApps.Order;
using ServerApp.SubApps.Serve;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ServerApp
{
	class Server
    {
		DatabaseLayer dbLayer = new DatabaseLayer();
        public void Start()
        {
				foreach (MyApplication subApp in dbLayer.GetSubApps())
				{
					if (subApp.IsRunning)
					{
						Console.WriteLine($"Starting: {subApp.TypeName}");
						StartVirtualDevices(subApp.Devices);
						switch (subApp.TypeName.TrimEnd(' '))
						{
							case "Inform":
								Thread t1 = new Thread(new ParameterizedThreadStart(RunSubApp));
								t1.Start(new InformSubApp(subApp.Devices.ToList(), subApp.AppName, dbLayer));
								break;
							case "Order":
								Thread t2 = new Thread(new ParameterizedThreadStart(RunSubApp));
								t2.Start(new OrderSubApp(subApp.Devices.ToList(), subApp.AppName, dbLayer));
								break;
							case "Serve":
								Thread t3 = new Thread(new ParameterizedThreadStart(RunSubApp));
								t3.Start(new ServeSubApp(subApp.Devices.ToList(), subApp.AppName, dbLayer));
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

		private void StartVirtualDevices(ICollection<Device> devices)
		{
			foreach (Device device in devices)
			{
				if (device.IP == "127.0.0.1")
					Process.Start(@"D:\git\build-Rallo-Desktop_Qt_5_10_0_MinGW_32bit-Release\release\RalloApp.exe", $"-e -p {device.Port}");
			}
		}

		private void RunSubApp(object o)
        {
			SubApp subApp = o as SubApp;
			subApp.Start();
		}
    }
}
