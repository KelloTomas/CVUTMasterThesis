using System.Timers;
using System.Collections.Generic;
using System.Globalization;

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

		public virtual void ProcessCardReadAction()
		{
		}
	}
}
