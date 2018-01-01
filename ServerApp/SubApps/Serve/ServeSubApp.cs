using ServerApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using ServerApp.SubApps.Shared.States;
using ServerApp.Devices;

namespace ServerApp.SubApps.Serve
{
	public class ServeSubApp : SubApp, IServeSubApp
	{
		public ServeSubApp(List<Device> devices) : base()
		{
		}
		

		public override IStateBase GetInitState()
        {
            return new DefaultState(200);
        }
		
    }
}
