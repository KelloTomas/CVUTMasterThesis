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
		public DefaultState(int timeout) : base(timeout)
		{
		}
		#endregion
	}
}