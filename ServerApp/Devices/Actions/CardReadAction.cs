using System;

namespace ServerApp.Devices.Actions
{
	public class CardReadAction : IAction
	{
		public CardReadAction(string cardNumber, DateTime? timeStamp = null)
		{
			CardNumber = cardNumber;
			TimeStamp = timeStamp;
		}
		public string CardNumber { get; }
		public DateTime? TimeStamp { get; }
	}
}
