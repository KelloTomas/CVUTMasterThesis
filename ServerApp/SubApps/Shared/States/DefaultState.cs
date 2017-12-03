using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;
using ServerApp.SubApps.Shared.States;

namespace ServerApp.SubApps.Shared.States
{
	public class DefaultState : StateBase
	{

		#region private fields...	
		#endregion

		#region constructors...
		public DefaultState(int timeout) : base(timeout)
		{
		}
		#endregion
	}
}