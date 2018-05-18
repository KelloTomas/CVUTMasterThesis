using System.Timers;
using System.Collections.Generic;
using System.Globalization;
using System;
using ServerApp.Devices.Actions;

namespace ServerApp.TerminalServices.Shared.States
{
	// definovanie zakladnych funkcii stavu a preddefinovanych prechodov
	public abstract class StateBase : IStateBase
	{
		private readonly ITerminalService _service;
		int _timerPeriod;
		public StateBase(ITerminalService service, int timerPeriod)
		{
			_service = service;
			_timerPeriod = timerPeriod;
		}

		public int TimeOut => _timerPeriod;

		public virtual void Enter()
		{
		}

		public virtual void Exit()
		{
		}

		// na zaklade druhu akcie vyvola potrebnu obsluhu prilozenia karty alebo stlacenie tlacidla
		public IStateBase ProcessAction(IAction action, ref bool forceCallStateMethod)
		{
			switch (action)
			{
				case ButtonClickAction buttonClickAction:
					Console.WriteLine($"{_service.GetType()} - ButtonClick with name: {buttonClickAction.ButtonName}");
					return ProcessButtonClickAction(buttonClickAction, ref forceCallStateMethod);
				case CardReadAction cardReadAction:
					Console.WriteLine($"{_service.GetType()} - Card number : {cardReadAction.CardNumber} was readed");
					return ProcessCardReadAction(cardReadAction, ref forceCallStateMethod);
				default:
					Console.WriteLine($"{_service.GetType()} - Unknow action happend: {action.GetType().Name}");
					return this;
			}
		}

		// funkcie ktorych chovanie by mala prepisat trieda ktora od tejto dedi
		public virtual IStateBase ProcessButtonClickAction(ButtonClickAction button, ref bool forceCallStateMethod)
		{
			Console.WriteLine($"{_service.GetType()} - No Response to button");
			return this;
		}

		public virtual IStateBase ProcessCardReadAction(CardReadAction card, ref bool forceCallStateMethod)
		{
			Console.WriteLine($"{_service.GetType()} - No Response to card read");
			return this;
		}

		public virtual IStateBase ProcessTimerElapsed()
		{
			return null;
		}
	}
}
