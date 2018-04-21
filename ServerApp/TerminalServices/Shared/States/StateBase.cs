using System.Timers;
using System.Collections.Generic;
using System.Globalization;
using System;
using ServerApp.Devices.Actions;

namespace ServerApp.TerminalServices.Shared.States
{
    public abstract class StateBase : IStateBase
    {
		int _timerPeriod;
        public StateBase(int timerPeriod)
        {
			_timerPeriod = timerPeriod;
        }

		public int TimeOut => _timerPeriod;

		public virtual void Enter()
		{
		}

		public virtual void Exit()
		{
		}


		public IStateBase ProcessAction(IAction action, ref bool forceCallStateMethod)
		{
			switch (action)
			{
				case ButtonClickAction buttonClickAction:
					return ProcessButtonClickAction(buttonClickAction, ref forceCallStateMethod);
				case CardReadAction cardReadAction:
					return ProcessCardReadAction(cardReadAction, ref forceCallStateMethod);
				default:
					Console.WriteLine($"Unknow action happend: {action.GetType().Name}");
					return this;
			}
		}
		
		public virtual IStateBase ProcessButtonClickAction(ButtonClickAction button, ref bool forceCallStateMethod)
		{
			Console.WriteLine("ProcessButtonClickQtEvenAction(): No Response");
			return this;
		}

		public virtual IStateBase ProcessCardReadAction(CardReadAction card, ref bool forceCallStateMethod)
		{
			Console.WriteLine("ProcessCardReadQtEventAction(): No Response");
			return this;
		}

		public virtual IStateBase ProcessTimerElapsed()
        {
            return null;
        }
    }
}
