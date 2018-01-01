using System;
using ServerApp.Data;

namespace ServerApp.Devices
{
	public class CardReadQtEventAction : IAction
	{

		public CardReadQtEventAction(string number, DateTime timeStamp)
		{
			Number = number;
			TimeStamp = timeStamp;
		}

		public string Number {get; private set; }
		public DateTime TimeStamp { get; private set; }

	}
}