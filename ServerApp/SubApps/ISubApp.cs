using ServerApp.Devices;
using ServerApp.SubApps.Shared.States;
using System;
using System.Collections.Generic;

namespace ServerApp.SubApps
{
	public interface ISubApp
	{
		IStateBase GetInitState();
        void Init(Action<IAction> processAction);
		void ProcessAction(IAction action);

		bool Terminated { get; set; }
		IStateBase ActualState { get; }
		IStateBase IdleState { get; }
	}
}
