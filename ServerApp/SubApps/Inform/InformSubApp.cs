using ServerApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace ServerApp.SubApps.Inform
{
	public class InformSubApp : SubApp, IInformSubApp
	{
		public InformSubApp() : base()
		{
		}
		public void SubscribeToActions(Action<IAction> processAction)
		{
			throw new NotImplementedException();
		}
	}
}
