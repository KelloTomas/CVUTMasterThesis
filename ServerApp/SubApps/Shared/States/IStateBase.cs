using ServerApp.Data;

namespace ServerApp.SubApps.Shared.States
{
	public interface IStateBase
	{
		void Enter();
		IStateBase ProcessTimerElapsed();
		IStateBase ProcessCardReadAction(CardReadAction c);
		IStateBase ProcessButtonClickAction(ButtonClickAction buttonClick);
		void Exit();
		int TimeOut {get;}
	}
}
