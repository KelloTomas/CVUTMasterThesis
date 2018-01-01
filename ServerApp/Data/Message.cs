using ServerApp.Data;
using ServerApp.Devices;
using System.Collections.Generic;
using System.Linq;

namespace ServerApp.Data
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
