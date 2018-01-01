using ServerApp.Data;
using ServerApp.Devices;
using System.Collections.Generic;

namespace ServerApp.Data
{
	/// <summary>
	/// Rozhraní pro zprávu.
	/// </summary>
	public interface IMessage
	{
		/*
		ISender Sender { get; }
		void AddSender(ISender sender);
		IEnumerable<ISender> Senders { get; }
		*/
		IAction[] Actions { get; }
	}
}
