using ServerApp.Devices.Actions;
using ServerApp.SubApps.Shared.States;
using System;
using System.Collections.Generic;

namespace ServerApp.SubApps
{
	public interface ISubApp
	{
		IStateBase GetInitState();
        void Init();
		void ProcessAction(IAction action);

		/// <summary>
		/// Akce, ktera se maji provest pri inicializaci HW
		/// </summary>
		IEnumerable<IAction> InitActions { get; }

		bool Terminated { get; set; }
		IStateBase ActualState { get; }
		IStateBase IdleState { get; }
	}
}
