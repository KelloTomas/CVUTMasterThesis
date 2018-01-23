namespace ServerApp.Devices.Actions
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
