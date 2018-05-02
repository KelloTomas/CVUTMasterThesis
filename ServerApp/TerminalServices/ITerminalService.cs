using ServerApp.Devices.Actions;
using ServerApp.TerminalServices.Shared.States;
using System;
using System.Collections.Generic;

namespace ServerApp.TerminalServices
{
	public interface ITerminalService
	{
		IStateBase GetInitState();
		void ProcessAction(IAction action);

		/// <summary>
		/// Akcie, ktere sa vykonaju po pripojeni na terminal
		/// </summary>
		IEnumerable<IAction> InitActions { get; }
		IStateBase ActualState { get; }
		IStateBase IdleState { get; }
	}
}
