using ServerApp.Data;
using ServerApp.SubApps.Shared.States;
using System;
using System.Collections.Generic;

namespace ServerApp.SubApps
{
	public interface ISubApp
	{
		IStateBase Start();
        void SubscribeToActions(Action<IAction> processAction);
	}
}
