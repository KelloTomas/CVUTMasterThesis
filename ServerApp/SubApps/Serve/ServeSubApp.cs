using System;
using System.Collections.Generic;
using DataLayer;
using DataLayer.Data;
using ServerApp.SubApps.Serve.States;
using ServerApp.SubApps.Shared.States;

namespace ServerApp.SubApps.Serve
{
	public class ServeSubApp : SubApp, IServeSubApp
	{
		public ServeSubApp(List<Device> devices, DatabaseLayer dbLayer) : base(dbLayer)
		{
		}
		

		public override IStateBase GetInitState()
        {
            return new SetServing(this);
        }
		
    }
}
