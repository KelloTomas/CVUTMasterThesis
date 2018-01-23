using ServerApp.Devices;
using ServerApp.Devices.Actions;
using ServerApp.SubApps;
using ServerApp.SubApps.Inform;
using ServerApp.SubApps.Order;
using ServerApp.SubApps.Serve;
using ServerApp.SubApps.Shared.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp
{
    class Server
    {
        List<ISubApp> SubApps = new List<ISubApp>();
        IStateBase currState;
        public void Start()
        {
            using (var db = new CVUTdbEntities())
                foreach (var subApp in db.Applications)
                {
                    if (subApp.IsRunning)
                    {
						Console.WriteLine($"Starting: {subApp.ApplicationType.Name}");
                        switch (subApp.ApplicationType.Name.TrimEnd(' '))
                        {
                            case "Inform":
                                Thread t1 = new Thread(new ParameterizedThreadStart(RunSubApp));
                                t1.Start(new InformSubApp(subApp.Devices.ToList()));
                                break;
                            case "Order":
                                Thread t2 = new Thread(new ParameterizedThreadStart(RunSubApp));
                                t2.Start(new OrderSubApp(subApp.Devices.ToList()));
                                break;
                            case "Serve":
                                Thread t3 = new Thread(new ParameterizedThreadStart(RunSubApp));
                                t3.Start(new ServeSubApp(subApp.Devices.ToList()));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
			Console.WriteLine("All apps started");
        }
        private void RunSubApp(object o)
        {
            SubApp subApp = o as SubApp;
            currState = subApp.GetInitState();
            subApp.Init();
            while (true)
            {
                IStateBase newState = currState.ProcessTimerElapsed();
				subApp.CheckNewState(newState);
                Console.WriteLine($"TimeElapsed: {subApp.GetType()}");
				Thread.Sleep(currState.TimeOut);
            }
        }
    }
}
