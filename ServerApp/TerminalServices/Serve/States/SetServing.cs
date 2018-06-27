using System.Collections.Generic;
using System.Linq;
using ServerApp.TerminalServices.Shared.States;
using ServerApp.Devices.Actions;
using DataLayer.Data;
using ServerApp.TerminalServices.Serve.Layouts;

namespace ServerApp.TerminalServices.Serve.States
{
	public class SetServing : StateBase
	{

		#region private fields...
		ServeTerminalService _app;
		private int _selected;
		List<Menu> _menu;
		#endregion

		#region constructors...
		public SetServing(ServeTerminalService subApp) : base(subApp, 3000)
		{
			_app = subApp;
			_menu = _app.databaseLayer.GetMenu().ToList();
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
					(_app.ClientTextLayout.SetTexts(_app.App.AppName))
				.Concat
					(_app.ClientTextLayout.SetContent("Nerikladajte kartu"))
				.ToArray())
				}.ToArray()
			));
			_app.ServiceDevice.SendMessage(new Message(
					new List<IAction> {

						new ShowLayoutAction(

						_app.SetServingLayout.Name,
				_app.ClientTextLayout.SetDateTimeTo()
				.Concat
					(_app.SetServingLayout.SetTitle(_app.App.AppName))
				.Concat
					(_app.SetServingLayout.SetMeal(_menu.Any()?_menu.ElementAt(_selected):null))
				.Concat
					(_app.SetServingLayout.SetNavigationButtons(_selected, _menu.Count))
				.ToArray())
				}.ToArray()
			));
			return this;
		}
		public override IStateBase ProcessButtonClickAction(ButtonClickAction button, ref bool forceCallStateMethod)
		{
			switch (button.ButtonName)
			{
				//zahajenie vydaja
				case SetServingLayout.Buttons.StartBtn:
					return new Serving(_app);

				//prechod medzi obrazovkami
				case SetServingLayout.Buttons.PrevBtn:
					if (_selected != 0)
						_selected--;
					break;
				case SetServingLayout.Buttons.NextBtn:
					if (_selected < _menu.Count - 1)
						_selected++;
					break;
				default:
					return base.ProcessButtonClickAction(button, ref forceCallStateMethod);
			}
			forceCallStateMethod = true;
			return this;
		}
	}
}