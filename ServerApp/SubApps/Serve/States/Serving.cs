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
	public class Serving : StateBase
	{

		#region private fields...
		ServeSubApp _app;
		string _msgConst = "Prilozte kartu";
		string _msg;
		#endregion

		#region constructors...
		public Serving(ServeSubApp subApp) : base(3000)
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

						_app.ClientTextLayout.Name,
				_app.ClientTextLayout.SetDateTimeTo()
				.Concat
					(_app.ClientTextLayout.SetTexts("Prebieha vydaj"))
				.Concat
					(_app.ClientTextLayout.SetContent(_msg))
				.ToArray())
				}.ToArray()
			));
			_msg = _msgConst;
			return this;
		}

		public override IStateBase ProcessCardReadAction(CardReadAction card, ref bool forceCallStateMethod)
		{
			Client c = _app.databaseLayer.GetClient(card.CardNumber);
			if (c != null)
			{
				Menu m = _app.databaseLayer.ServeOrder(c.Id);
				if (m != null)
				{
					return new Served(_app, m);
				}
				else
					_msg = "Nemate ziadne objednavky";
			}
			else
			{
				_msg = "Neznama karta";
			}
			return this;
		}
	}
}