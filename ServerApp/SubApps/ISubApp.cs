using ServerApp.Data;
using ServerApp.SubApps.Shared.States;
using System;

namespace ServerApp.SubApps
{
	public interface ISubApp
	{
		IStateBase Init();
		void SubscribeToActions(Action<IAction> processAction);
	}
}
