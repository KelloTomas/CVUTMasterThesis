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
using ServerApp.TerminalServices.Serve.Layouts;

namespace ServerApp.TerminalServices.Serve.States
{
	public class Serving : StateBase
	{

		#region private fields...
		ServeTerminalService _app;
		string _msgConst = "Prilozte kartu";
		string _msg;
		Client _client;
		#endregion

		#region constructors...
		public Serving(ServeTerminalService subApp) : base(3000)
		{
			_app = subApp;
			_msg = _msgConst;
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
					(_app.ClientTextLayout.SetTexts("Prebieha vydaj"))
				.Concat
					(_app.ClientTextLayout.SetContent(_msg))
				.ToArray())
				}.ToArray()
			));
			_app.ServiceDevice.SendMessage(new Message(
					new List<IAction> {

						new ShowLayoutAction(

						_app.ServingLayout.Name,
				_app.ServingLayout.SetDateTimeTo()
				.Concat
					(_app.ServingLayout.SetMenu(null, _client, _msg == _msgConst?_app.ServiceName:_msg))
				.ToArray())
				}.ToArray()
			));
			_msg = _msgConst;
			_client = null;
			return this;
		}

		public override IStateBase ProcessCardReadAction(CardReadAction card, ref bool forceCallStateMethod)
		{
			_client = _app.databaseLayer.GetClient(card.CardNumber);
			if (_client != null)
			{
				Menu m = _app.databaseLayer.ServeOrder(_client.Id);
				if (m != null)
				{
					return new Served(_app, m, _client);
				}
				else
					_msg = "Nemate ziadne objednavky";
			}
			else
			{
				_msg = "Neznama karta";
			}
			forceCallStateMethod = true;
			return this;
		}
		public override IStateBase ProcessButtonClickAction(ButtonClickAction button, ref bool forceCallStateMethod)
		{
			switch (button.ButtonName)
			{
				case ServingLayout.Buttons.StopBtn:
					return new SetServing(_app);
				case ServingLayout.Buttons.ShowServedBtn:
					return new History(_app);
			}
			return base.ProcessButtonClickAction(button, ref forceCallStateMethod);
		}
	}
}