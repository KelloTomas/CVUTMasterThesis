using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;
using ServerApp.TerminalServices.Shared.States;
using ServerApp.Devices;
using ServerApp.TerminalServices.Shared.Data;
using ServerApp.TerminalServices.Shared.Layouts;
using ServerApp.Devices.Actions;
using ServerApp.TerminalServices.Order.Layouts;
using DataLayer.Data;

namespace ServerApp.TerminalServices.Order.States
{
    public class OrderState : StateBase
    {

        #region private fields...
        List<Menu> _menuOnScreen;
        OrderTerminalService _app;
        LayoutBase _layout;
        Client _client;
        DateTime _dateToOrder;
        private string _clientMsg = null;
        IEnumerable<Menu> _menu;
        private int _pageNum;
        private int? _selected;
        bool _reset = true;
        #endregion

        #region constructors...
        public OrderState(OrderTerminalService subApp) : base(3000)
        {
            _app = subApp;
        }
        #endregion
        public override void Enter()
        {
            base.Enter();
            _menu = _app.databaseLayer.GetMenu();
            SetInitValues();
        }

        private void SetInitValues()
        {
            _pageNum = 0;
            _selected = null;
            SetDateToOrder(DateTime.Now.Date);
            _layout = _app.OrdersLayout;
            _clientMsg = null;
        }

        private void SetDateToOrder(DateTime date)
        {
            _dateToOrder = date;
            _menuOnScreen = _menu.Where(m => m.ForDate == _dateToOrder).ToList();
        }

        public override IStateBase ProcessTimerElapsed()
        {
            List<IAction> action;
            switch (_layout)
            {
                case OrdersLayout ordersLayout:
                    action = new List<IAction>
                    {
                        new ShowLayoutAction(ordersLayout.Name,
                        SetDateTimeToNow()
                        .Concat
                            (ordersLayout.SetDate(_dateToOrder, _menu.Where(m => m.ForDate > _dateToOrder).Any()))
                        .Concat
                            (ordersLayout.SetMeals(_menuOnScreen, _pageNum, _selected.HasValue ? _selected.GetValueOrDefault() % _app.OrdersLayout.MEALS_PER_PAGE : _selected))
                        .ToArray())
                    };
                    break;
                case MessageLayout messageLayout:
                    action = new List<IAction>
                    {
                        new ShowLayoutAction(messageLayout.Name,
                        SetDateTimeToNow()
                        .Concat
                            (messageLayout.SetText(_client, _clientMsg))
                        .ToArray()),
                    };
                    break;
                default:
                    action = new List<IAction>();
                    break;
            }
            _app.Rallo.SendMessage(new Message(action.ToArray()));
            if (_reset)
            {
                SetInitValues();
            }
            else
            {
                _reset = true;
            }
            return this;
        }
        public override IStateBase ProcessButtonClickAction(ButtonClickAction button, ref bool forceCallStateMethod)
        {
            if (button.ButtonName.Contains("menu_"))
            {
                _selected = int.Parse(button.ButtonName.Last().ToString()) + _pageNum * _app.OrdersLayout.MEALS_PER_PAGE;
            }
            else
            {
                switch (button.ButtonName)
                {
                    case "prevDay":
                        SetDateToOrder(_dateToOrder.AddDays(-1));
                        break;
                    case "nextDay":
                        SetDateToOrder(_dateToOrder.AddDays(1));
                        break;
                    case "up":
                        _pageNum--;
                        break;
                    case "down":
                        _pageNum++;
                        break;
                    default:
                        return base.ProcessButtonClickAction(button, ref forceCallStateMethod);
                }
                _selected = null;
            }
            _reset = false;
            forceCallStateMethod = true;
            return this;
        }

        public override IStateBase ProcessCardReadAction(CardReadAction card, ref bool forceCallStateMethod)
        {
            _client = _app.databaseLayer.GetClient(card.CardNumber);
            //app.db.Cards.Where(c => c.CardNumber == card.CardNumber).FirstOrDefault()?.Client;
            _layout = _app.MessageLayout;
            if (_client == null)
            {
                _clientMsg = "Neznama karta";
            }
            else
            {
                if (_selected == null)
                    _clientMsg = "Nevybrali ste jedlo";
                else
                {
                    Menu menu = _menuOnScreen[_selected.GetValueOrDefault()];
                    bool success;
                    (success, _clientMsg) = _app.databaseLayer.CreateOrder(_client, menu);
                }
            }
            forceCallStateMethod = true;
            return this;
        }

        private IEnumerable<ModifyLayoutItem> SetDateTimeToNow()
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