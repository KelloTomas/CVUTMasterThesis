using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;
using ServerApp.SubApps.Shared.States;

namespace ServerApp.SubApps.Inform.States
{
	public class InformState : StateBaseWithTimers
	{

		#region private fields...	
		#endregion

		#region constructors...
		public InformState(int timeout) : base(timeout)
		{
		}
		#endregion

	}
}