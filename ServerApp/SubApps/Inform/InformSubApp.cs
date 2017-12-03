using ServerApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using ServerApp.SubApps.Shared.States;
using ServerApp.SubApps.Inform.States;

namespace ServerApp.SubApps.Inform
{
	public class InformSubApp : SubApp, IInformSubApp
	{
        Rasllo rasllo;

		public InformSubApp(List<Device> devices) : base()
		{
            if (devices.Count != 1)
                throw new ArgumentOutOfRangeException();
            rasllo = new Rasllo(devices[0]);
        }

        public override IStateBase Start()
        {
            return new InformState(200);
        }

        public override void SubscribeToActions(Action<IAction> processAction)
        {
        }
    }
}
