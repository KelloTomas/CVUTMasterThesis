using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;
using ServerApp.SubApps.Shared.States;
using ServerApp.Data;
using ServerApp.Devices;
using ServerApp.SubApps.Shared.Data;

namespace ServerApp.SubApps.Inform.States
{
	public class InformState : StateBase
	{

		#region private fields...
		InformSubApp app;
		#endregion

		#region constructors...
		public InformState(InformSubApp subApp, int timeout) : base(timeout)
		{
			app = subApp;
		}
		#endregion
		public override IStateBase ProcessTimerElapsed()
		{
			Console.WriteLine("ToDo processs time elapsed");
			List<IAction> action = new List<IAction>
				{
					new ShowLayoutAction("TestToDo", SetDateTimeToNow().ToArray())
				};
			app.rallo.SendMessage(new Message(action.ToArray()));
			return this;
		}
		public IEnumerable<ModifyLayoutItem> SetDateTimeToNow()
		{
			return SetDateTimeTo(DateTime.Now);
		}
		private IEnumerable<ModifyLayoutItem> SetDateTimeTo(DateTime dateTime)
		{
			yield return new ModifyLayoutItem("DateValue", "text", dateTime.ToString("d"));
			yield return new ModifyLayoutItem("TimeValue", "text", dateTime.ToString("t"));
		}
	}
}