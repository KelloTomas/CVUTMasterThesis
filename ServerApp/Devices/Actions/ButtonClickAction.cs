using System;

namespace ServerApp.Devices.Actions
{
	public class ButtonClickAction : IAction
	{
		public ButtonClickAction(string buttonName, DateTime? timeStamp = null)
		{
			ButtonName = buttonName;
			TimeStamp = timeStamp;
		}

		public string ButtonName { get; }
		public DateTime? TimeStamp { get; }
	}
}
