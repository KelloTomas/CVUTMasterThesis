using System;
using ServerApp.Data;
using ServerApp.SubApps.Shared.States;

namespace ServerApp.SubApps
{
	public class SubApp : ISubApp
	{
		public SubApp()
		{
		}

        public virtual IStateBase Start()
        {
            throw new NotImplementedException();
        }

        public virtual void SubscribeToActions(Action<IAction> processAction)
        {
            throw new NotImplementedException();
        }
    }
}
