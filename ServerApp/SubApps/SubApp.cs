using System;
using ServerApp.Data;
using ServerApp.Devices;
using ServerApp.SubApps.Shared.States;

namespace ServerApp.SubApps
{
	public class SubApp : ISubApp
	{
		public SubApp()
		{
		}

		public bool Terminated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public IStateBase ActualState => throw new NotImplementedException();

		public IStateBase IdleState => throw new NotImplementedException();

		public void ProcessAction(IAction action)
		{
			throw new NotImplementedException();
		}

		public virtual IStateBase GetInitState()
        {
            throw new NotImplementedException();
        }

		public virtual void Init(Action<IAction> processAction)
		{
			throw new NotImplementedException();
		}

		IStateBase ISubApp.GetInitState()
		{
			throw new NotImplementedException();
		}
	}
}
