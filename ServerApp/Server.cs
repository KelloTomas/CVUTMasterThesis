using ServerApp.Data;
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
        }
        private void RunSubApp(object o)
        {
            ISubApp subApp = o as ISubApp;
            currState = subApp.Start();
            subApp.SubscribeToActions(ProcessAction);
            while (true)
            {
                IStateBase newState = currState.ProcessTimerElapsed();
                CheckNewState(newState);
                Thread.Sleep(1000);
                Console.WriteLine($"Running: {subApp.GetType()}");
            }
        }
        private void ProcessAction(IAction action)
        {
            IStateBase newState;
            switch (action)
            {
                case ButtonClickAction b:
                    newState = currState.ProcessButtonClickAction(b);
                    break;
                case CardReadAction c:
                    newState = currState.ProcessCardReadAction(c);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            CheckNewState(newState);
        }
        private void CheckNewState(IStateBase newState)
        {
            if (newState != null)
            {
                currState.Exit();
                newState.Enter();
                currState = newState;
            }
        }
    }
}
