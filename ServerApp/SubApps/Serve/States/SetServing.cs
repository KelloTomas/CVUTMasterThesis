using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;
using ServerApp.SubApps.Shared.States;
using ServerApp.Devices;
using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using ServerApp.Devices.Actions;
using System.Data.Entity;
using DataLayer.Data;

namespace ServerApp.SubApps.Serve.States
{
	public class SetServing : StateBase
	{

        #region private fields...
        ServeSubApp _app;
		private int _selected;
		#endregion

		#region constructors...
		public SetServing(ServeSubApp subApp) : base(3000)
		{
			_app = subApp;
		}
		#endregion

		public override IStateBase ProcessTimerElapsed()
		{
			_app.ClientDevice.SendMessage(new Message(
					new List<IAction> {

						new ShowLayoutAction(

						_app.ClientTextLayout.Name,
				_app.ClientTextLayout.SetDateTimeTo()
				.Concat
					(_app.ClientTextLayout.SetTexts("KLIENT"))
				.Concat
					(_app.ClientTextLayout.SetContent("Prilozte kartu"))
				.ToArray())
				}.ToArray()
			));
			_app.ServiceDevice.SendMessage(new Message(
					new List<IAction> {

						new ShowLayoutAction(

						_app.ClientTextLayout.Name,
				_app.ClientTextLayout.SetDateTimeTo()
				.Concat
					(_app.ClientTextLayout.SetTexts("SERVIS"))
				.Concat
					(_app.ClientTextLayout.SetContent("Prilozte kartu"))
				.ToArray())
				}.ToArray()
			));
			return this;
		}
		public override IStateBase ProcessButtonClickAction(ButtonClickAction button, ref bool forceCallStateMethod)
		{
			if (button.ButtonName.Contains("btn_"))
			{
				_selected = int.Parse(button.ButtonName.Last().ToString());
			}
			else
			{
				switch (button.ButtonName)
				{
					case "prevDay":
						break;
					default:
						return base.ProcessButtonClickAction(button, ref forceCallStateMethod);
				}
			}
			forceCallStateMethod = true;
			return this;
		}

		public override IStateBase ProcessCardReadAction(CardReadAction card, ref bool forceCallStateMethod)
		{
			forceCallStateMethod = true;
			return this;
		}
	}
}