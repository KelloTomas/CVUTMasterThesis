using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;
using ServerApp.TerminalServices.Shared.States;
using ServerApp.Devices;
using ServerApp.TerminalServices.Shared.Data;
using ServerApp.TerminalServices.Shared.Layouts;
using ServerApp.Devices.Actions;
using System.Data.Entity;
using DataLayer.Data;

namespace ServerApp.TerminalServices.Serve.States
{
	public class Served : StateBase
	{

		#region private fields...
		ServeTerminalService _app;
		private readonly Menu _menu;
		private readonly Client _client;
		#endregion

		#region constructors...
		public Served(ServeTerminalService subApp, Menu menu, Client client) : base(6000)
		{
			_app = subApp;
			_menu = menu;
			_client = client;
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

						_app.ClientServedLayout.Name,
				_app.ClientServedLayout.SetDateTimeTo()
				.Concat
					(_app.ClientServedLayout.SetMenu(_menu, _client, null))
				.ToArray())
				}.ToArray()
			));
			_app.ServiceDevice.SendMessage(new Message(
					new List<IAction> {

						new ShowLayoutAction(
						_app.ServingLayout.Name,
				_app.ServingLayout.SetDateTimeTo()
				.Concat
					(_app.ServingLayout.SetMenu(_menu, _client, null))
				.ToArray())
				}.ToArray()
			));
			return new Serving(_app);
		}
	}
}