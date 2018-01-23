using System.Linq;

namespace ServerApp.Devices.Actions
{
	/// <summary>
	/// Rozhraní pro zprávu.
	/// </summary>
	public class Message : IMessage
	{
		private readonly IAction[] _actions;

		public Message(IAction[] actions)
		{
			_actions = actions;
		}

		public IAction[] Actions
		{
			get { return _actions.ToArray(); }
		}
	}
}
