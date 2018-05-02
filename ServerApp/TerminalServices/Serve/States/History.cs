using System.Collections.Generic;
using System.Linq;
using ServerApp.TerminalServices.Shared.States;
using ServerApp.Devices.Actions;
using ServerApp.TerminalServices.Serve.Layouts;

namespace ServerApp.TerminalServices.Serve.States
{
	public class History : StateBase
	{

		#region private fields...
		ServeTerminalService _app;
		private int _selected;
		List<DataLayer.Data.Order> _orders;
		#endregion

		#region constructors...
		public History(ServeTerminalService subApp) : base(6000)
		{
			_app = subApp;
		}
		#endregion

		public override void Enter()
		{
			base.Enter();
			// pri vstupe nacitam vydane objednavky a zobrazim
			_orders = _app.databaseLayer.GetServedOrders(10).ToList();
			ProcessTimerElapsed();
		}

		public override IStateBase ProcessTimerElapsed()
		{
			//obnova obrazovky klienta a obsluhy
			_app.ClientDevice.SendMessage(new Message(
					new List<IAction> {

						new ShowLayoutAction(

						_app.ClientTextLayout.Name,
				_app.ClientTextLayout.SetDateTimeTo()
				.Concat
					(_app.ClientTextLayout.SetTexts(_app.App.AppName))
				.Concat
					(_app.ClientTextLayout.SetContent("Nerikladajte kartu"))
				.ToArray())
				}.ToArray()
			));

			_app.ServiceDevice.SendMessage(new Message(
					new List<IAction> {

						new ShowLayoutAction(
						_app.HistoryLayout.Name,
				_app.HistoryLayout.SetDateTimeTo()
				.Concat
					(_app.HistoryLayout.SetMenu(_orders.Any()?_orders.ElementAt(_selected):null))
				.Concat
					(_app.HistoryLayout.SetNavigationButtons(_selected, _orders.Count))
				.ToArray())
				}.ToArray()
			));
			return this;
		}
		public override IStateBase ProcessButtonClickAction(ButtonClickAction button, ref bool forceCallStateMethod)
		{
			// obsluha stlacenia tlacidla
			switch (button.ButtonName)
			{
				case HistoryLayout.Buttons.RemoveBtn:
					_app.ServiceDevice.SendMessage(new Message(
							new List<IAction> {

						new ModalWindowShowAction("Určite si prajete vrátiť výdaj?", new ModalWindowButton[2] {new ModalWindowButton(){BtnId = "a", BtnText = "Ano" }, new ModalWindowButton(){ BtnId = "a", BtnText = "Nie" } })
						}.ToArray()
					));
					break;
				case HistoryLayout.Buttons.BackBtn:
					return new Serving(_app);
				case HistoryLayout.Buttons.PrevBtn:
					if (_selected != 0)
						_selected--;
					break;
				case HistoryLayout.Buttons.NextBtn:
					if (_selected < _orders.Count - 1)
						_selected++;
					break;
				case "a":
					_app.ServiceDevice.SendMessage(new Message(
							new List<IAction> {

						new ModalWindowCloseAction()
						}.ToArray()
					));
					break;
				default:
					return base.ProcessButtonClickAction(button, ref forceCallStateMethod);
			}
			forceCallStateMethod = true;
			return this;
		}
	}
}