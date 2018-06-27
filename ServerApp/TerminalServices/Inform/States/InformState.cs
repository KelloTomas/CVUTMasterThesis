using System.Collections.Generic;
using System;
using System.Linq;
using DataLayer.Data;
using ServerApp.TerminalServices.Shared.States;
using ServerApp.TerminalServices.Shared.Data;
using ServerApp.TerminalServices.Shared.Layouts;
using ServerApp.Devices.Actions;
using ServerApp.TerminalServices.Inform.Layouts;

namespace ServerApp.TerminalServices.Inform.States
{
	public class InformState : StateBase
	{

		#region private fields...
		InformTerminalService _app;
		LayoutBase _layout;
		Client _client;
		private string _clientMsg = null;
		#endregion

		#region constructors...
		public InformState(InformTerminalService subApp) : base(subApp, 10000)
		{
			_app = subApp;
			_layout = _app.ScanCardLayout;
		}
		#endregion
		public override IStateBase ProcessTimerElapsed()
		{
			List<IAction> action;
			switch (_layout)
			{
				case ScanCardLayout scanCard:
					action = new List<IAction>
					{
						new ShowLayoutAction(scanCard.Name,
						SetDateTimeToNow()
						.Concat
							(scanCard.SetTexts("Informácie o objednávkach", _clientMsg??"Priložte kartu"))
						.ToArray())
					};
					break;
				case CardScannedLayout cardScanned:
					action = new List<IAction>
					{
						new ShowLayoutAction(cardScanned.Name,
						SetDateTimeToNow()
						.Concat
							(cardScanned.SetMeals(_client))
						.ToArray()),
					};
					break;
				default:
					action = new List<IAction>();
					break;
			}
			_app.Rallo.SendMessage(new Message(action.ToArray()));
			_layout = _app.ScanCardLayout;
			_clientMsg = null;
			return this;
		}
		public override IStateBase ProcessButtonClickAction(ButtonClickAction button, ref bool forceCallStateMethod)
		{
			_layout = _app.CardScannedLayout;
			return this;
		}

		public override IStateBase ProcessCardReadAction(CardReadAction card, ref bool forceCallStateMethod)
		{
			_client = _app.databaseLayer.GetOrders(card.CardNumber);
			if (_client == null)
			{
				_clientMsg = "Neznáma karta";
			}
			else
			{
				_layout = _app.CardScannedLayout;
				_client.Orders = _client.Orders.Where(o => o.Served == null && o.ForDate.Date >= DateTime.Now.Date).ToList();
			}
			forceCallStateMethod = true;
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