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
	public class Served : StateBase
	{

        #region private fields...
        ServeSubApp _app;
		private readonly Menu _vydane;
		#endregion

		#region constructors...
		public Served(ServeSubApp subApp, Menu vydane) : base(3000)
		{
			_app = subApp;
			_vydane = vydane;
		}
		#endregion
		public override void Enter()
        {
            base.Enter();
			ProcessTimerElapsed();
        }


		public override IStateBase ProcessTimerElapsed()
		{
			_app.ClientDevice.SendMessage(new Message(
					new List<IAction> {

						new ShowLayoutAction(

						_app.ClientTextLayout.Name,
				_app.ClientTextLayout.SetDateTimeTo()
				.Concat
					(_app.ClientTextLayout.SetTexts("Vydane jedlo"))
				.Concat
					(_app.ClientTextLayout.SetContent(_vydane.ForDate.ToString()))
				.ToArray())
				}.ToArray()
			));
			_app.ServiceDevice.SendMessage(new Message(
					new List<IAction> {

						new ShowLayoutAction(

						_app.ClientTextLayout.Name,
				_app.ClientTextLayout.SetDateTimeTo()
				.Concat
					(_app.ClientTextLayout.SetTexts("Vydane jedlo"))
				.Concat
					(_app.ClientTextLayout.SetContent($"{_vydane.Items[0].Name} {_vydane.Items[0].Description}, {_vydane.Items[1].Name} {_vydane.Items[1].Description}, {_vydane.Items[2].Name} {_vydane.Items[2].Description}"))
				.ToArray())
				}.ToArray()
			));
			return new Serving(_app);
		}
	}
}