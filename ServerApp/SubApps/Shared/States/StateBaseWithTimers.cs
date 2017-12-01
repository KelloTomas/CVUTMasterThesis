using System;
using System.Timers;

namespace ServerApp.SubApps.Shared.States
{
		
	public abstract class StateBaseWithTimers : StateBase
	{
		#region private fields...				
		private readonly Timer _timerShowText;
		private bool _disposed = false;
		private readonly Timer _timerDisalbeCardScan;
		#endregion

		#region constructors...
		public StateBaseWithTimers(double timerPeriod)
			: base(timerPeriod)
		{
		}
#endregion
	}
}
