using System.Timers;
using System.Collections.Generic;
using System.Globalization;
using ServerApp.Data;

namespace ServerApp.SubApps.Shared.States
{
    public abstract class StateBase : IStateBase
    {
        public StateBase(double timerPeriod)
        {
        }

		public virtual void Enter()
		{
		}

		public virtual void Exit()
		{
		}

		public virtual void ProcessButtonClickAction()
		{
		}

        public virtual IStateBase ProcessButtonClickAction(ButtonClickAction buttonClick)
        {
            return null;
        }

        public virtual void ProcessCardReadAction()
		{
		}

        public virtual IStateBase ProcessCardReadAction(CardReadAction c)
        {
            return null;
        }

        public virtual IStateBase ProcessTimerElapsed()
        {
            return null;
        }
    }
}
