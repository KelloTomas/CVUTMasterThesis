using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;
using ServerApp.TerminalServices.Shared.States;

namespace ServerApp.TerminalServices.Shared.States
{
	public class DefaultState : StateBase
	{

		#region private fields...	
		#endregion

		#region constructors...
		public DefaultState(TerminalService subApp, int timeout) : base(subApp, timeout)
		{
		}
		#endregion
	}
}