using ServerApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using ServerApp.SubApps.Shared.States;
using ServerApp.SubApps.Inform.States;
using ServerApp.Devices;

namespace ServerApp.SubApps.Inform
{
	public class InformSubApp : SubApp, IInformSubApp
	{
        public Rallo rallo {get; private set;}

		public InformSubApp(List<Device> devices) : base()
		{
            if (devices.Count != 1)
                throw new ArgumentOutOfRangeException();
            rallo = new Rallo();
			//rallo.Connect(devices[0].IP, devices[0].Port ?? 15000);
			rallo.Connect("127.0.0.1", 15000);
		}
		public override void Init(Action<IAction> processAction)
		{
		}
		public override IStateBase GetInitState()
        {
            return new InformState(this, 200);
        }
		public void ProcessAction(Action action)
		{

		}
	}
}
