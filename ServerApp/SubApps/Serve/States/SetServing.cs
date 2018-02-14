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
        List<Menu> menuOnScreen;
        ServeSubApp app;
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
		public SetServing(ServeSubApp subApp) : base(3000)
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
            clientMsg = null;
        }

        private void SetDateToOrder(DateTime date)
        {
            dateToOrder = date;
            menuOnScreen = menu.Where(m => m.ForDate == dateToOrder).ToList();
        }

        public override IStateBase ProcessTimerElapsed()
		{
			/*
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
			*/
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