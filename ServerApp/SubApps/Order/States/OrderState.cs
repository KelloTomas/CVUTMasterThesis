using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;
using ServerApp.SubApps.Shared.States;
using ServerApp.Devices;
using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using ServerApp.Devices.Actions;
using ServerApp.SubApps.Order.Layouts;
using System.Data.Entity;
using ServerApp.Data;

namespace ServerApp.SubApps.Order.States
{
	public class OrderState : StateBase
	{

        #region private fields...
        List<Menu> menuOnScreen;
        OrderSubApp app;
		LayoutBase layout;
		Client client;
		DateTime dateToOrder;
		private string clientMsg = null;
		IEnumerable<Menu> menu;
		private int pageNum;
		private int? selected;
        bool reset = true;
		#endregion

		#region constructors...
		public OrderState(OrderSubApp subApp) : base(3000)
		{
			app = subApp;
		}
		#endregion
		public override void Enter()
        {
            base.Enter();
			menu = app.databaseLayer.GetMenu();
			SetInitValues();
        }

        private void SetInitValues()
        {
            pageNum = 0;
            selected = null;
            SetDateToOrder(DateTime.Now.Date);
            layout = app.OrdersLayout;
            clientMsg = null;
        }

        private void SetDateToOrder(DateTime date)
        {
            dateToOrder = date;
            menuOnScreen = menu.Where(m => m.ForDate == dateToOrder).ToList();
        }

        public override IStateBase ProcessTimerElapsed()
		{
			List<IAction> action;
			switch (layout)
			{
				case OrdersLayout ordersLayout:
					action = new List<IAction>
					{
						new ShowLayoutAction(ordersLayout.Name,
						SetDateTimeToNow()
						.Concat
							(ordersLayout.SetDate(dateToOrder, menu.Where(m => m.ForDate > dateToOrder).Any()))
						.Concat
							(ordersLayout.SetMeals(menuOnScreen, pageNum, selected.HasValue ? selected.GetValueOrDefault() % app.OrdersLayout.MEALS_PER_PAGE : selected))
						.ToArray())
					};
					break;
				case MessageLayout messageLayout:
					action = new List<IAction>
					{
						new ShowLayoutAction(messageLayout.Name,
						SetDateTimeToNow()
						.Concat
							(messageLayout.SetText(client, clientMsg))
						.ToArray()),
					};
					break;
				default:
					action = new List<IAction>();
					break;
			}
			app.Rallo.SendMessage(new Message(action.ToArray()));
            if (reset)
            {
                SetInitValues();
            }
            else
            {
                reset = true;
            }
			return this;
		}
		public override IStateBase ProcessButtonClickAction(ButtonClickAction button, ref bool forceCallStateMethod)
		{
            if (button.ButtonName.Contains("menu_"))
            {
                selected = int.Parse(button.ButtonName.Last().ToString()) + pageNum * app.OrdersLayout.MEALS_PER_PAGE;
            }
            else
            {
                switch (button.ButtonName)
                {
                    case "prevDay":
						SetDateToOrder(dateToOrder.AddDays(-1));
                        break;
                    case "nextDay":
						SetDateToOrder(dateToOrder.AddDays(1));
						break;
                    case "up":
                        pageNum--;
                        break;
                    case "down":
                        pageNum++;
                        break;
                    default:
			            return base.ProcessButtonClickAction(button, ref forceCallStateMethod);
                }
                selected = null;
            }
            reset = false;
            forceCallStateMethod = true;
            return this;
		}

		public override IStateBase ProcessCardReadAction(CardReadAction card, ref bool forceCallStateMethod)
		{
			client = app.databaseLayer.GetClient(card.CardNumber);
				//app.db.Cards.Where(c => c.CardNumber == card.CardNumber).FirstOrDefault()?.Client;
			layout = app.MessageLayout;
			if (client == null)
			{
				clientMsg = "Neznama karta";
			}
			else
			{
				if (selected == null)
					clientMsg = "Nevybrali ste jedlo";
				else
				{
					/*
					 * ToDo
					app.db.Orders.Add(new ServerApp.Order() { ForDate = dateToOrder, IdClient = client.IdClient, IdMenu = menuOnScreen[selected.GetValueOrDefault()].IdMenu });
					app.db.SaveChanges();
					*/
					app.databaseLayer.AddOrder(client.Id, dateToOrder, menuOnScreen[selected.GetValueOrDefault()].IdMenu);
					//throw new NotImplementedException();
					clientMsg = "Objednane";
				}
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