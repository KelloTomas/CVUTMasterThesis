using ServerApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using ServerApp.SubApps.Shared.States;

namespace ServerApp.SubApps.Order
{
	public class OrderSubApp : SubApp, IOrderSubApp
	{
		public OrderSubApp(List<Device> devices) : base()
		{
		}

        public override IStateBase Start()
        {
            return new DefaultState(200);
        }

        public override void SubscribeToActions(Action<IAction> processAction)
        {

        }
    }
}
