using System;
using ServerApp.Data;

namespace ServerApp.Devices
{
	internal class ButtonClickQtEvenAction : IAction
	{
		private string id;
		private DateTime timeStamp;

		public ButtonClickQtEvenAction(string id, DateTime timeStamp)
		{
			this.id = id;
			this.timeStamp = timeStamp;
		}
	}
}