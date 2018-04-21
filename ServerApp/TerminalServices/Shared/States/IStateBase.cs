using ServerApp.Devices.Actions;

namespace ServerApp.TerminalServices.Shared.States
{
	public interface IStateBase
	{
		void Enter();
		IStateBase ProcessTimerElapsed();
		/*
		IStateBase ProcessCardReadAction(CardReadAction c);
		IStateBase ProcessButtonClickAction(ButtonClickAction buttonClick);
		*/
		IStateBase ProcessAction(IAction action, ref bool forceCallStateMethod);
		void Exit();
		int TimeOut {get;}
	}
}
