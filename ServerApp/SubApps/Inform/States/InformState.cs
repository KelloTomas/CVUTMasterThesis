using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;
using ServerApp.SubApps.Shared.States;
using ServerApp.Devices;
using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using ServerApp.Devices.Actions;
using ServerApp.SubApps.Inform.Layouts;

namespace ServerApp.SubApps.Inform.States
{
	public class InformState : StateBase
	{

		#region private fields...
		InformSubApp app;
		LayoutBase layout;
		Data.Client client;
		private string clientMsg = null;
		#endregion

		#region constructors...
		public InformState(InformSubApp subApp) : base(10000)
		{
			app = subApp;
			layout = app.ScanCardLayout;
		}
		#endregion
		public override IStateBase ProcessTimerElapsed()
		{
			List<IAction> action;
			switch (layout)
			{
				case ScanCardLayout scanCard:
					action = new List<IAction>
					{
						new ShowLayoutAction(scanCard.Name,
						SetDateTimeToNow()
						.Concat
							(scanCard.SetTexts(" ", clientMsg??"Prilozte kartu"))
						.ToArray())
					};
					break;
				case CardScannedLayout cardScanned:
					action = new List<IAction>
					{
						new ShowLayoutAction(cardScanned.Name,
						SetDateTimeToNow()
						.Concat
							(cardScanned.SetMeals(client))
						.ToArray()),
					};
					break;
				default:
					action = new List<IAction>();
					break;
			}
			app.Rallo.SendMessage(new Message(action.ToArray()));
			layout = app.ScanCardLayout;
			clientMsg = null;
			return this;
		}
		public override IStateBase ProcessButtonClickAction(ButtonClickAction button, ref bool forceCallStateMethod)
		{
			layout = app.CardScannedLayout;
			return this;
		}

		public override IStateBase ProcessCardReadAction(CardReadAction card, ref bool forceCallStateMethod)
		{
			client = app.databaseLayer.GetOrders(card.CardNumber);
			if (client == null)
			{
				clientMsg = "Neznama karta";
			}
			else
			{
				layout = app.CardScannedLayout;
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