using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;
using ServerApp.SubApps.Shared.States;
using ServerApp.Devices;
using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using ServerApp.Devices.Actions;

namespace ServerApp.SubApps.Inform.States
{
	public class InformState : StateBase
	{

		#region private fields...
		InformSubApp app;
		LayoutBase layout;
		#endregion

		#region constructors...
		public InformState(InformSubApp subApp, int timeout) : base(timeout)
		{
			app = subApp;
			layout = app.ScanCardLayout;
		}
		#endregion
		public override IStateBase ProcessTimerElapsed()
		{
			Console.WriteLine("ToDo processs time elapsed");
			List<IAction> action = new List<IAction>
				{
					new ShowLayoutAction(layout.Name, SetDateTimeToNow().ToArray())
				};
			app.Rallo.SendMessage(new Message(action.ToArray()));
			layout = app.ScanCardLayout;
			return this;
		}
		public override IStateBase ProcessButtonClickAction(ButtonClickAction button, ref bool forceCallStateMethod)
		{
			layout = app.CardScannedLayout;
			return base.ProcessButtonClickAction(button, ref forceCallStateMethod);
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