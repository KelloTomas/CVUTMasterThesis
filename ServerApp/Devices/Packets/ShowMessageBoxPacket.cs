using ServerApp.Devices.Actions;
using System.Xml.Linq;

namespace ServerApp.Devices.Packets
{
	internal class ShowMessageBoxPacket : BasePacket
	{
		private readonly IShowMessageBoxAction _action;

		public ShowMessageBoxPacket(IShowMessageBoxAction action)
		{
			_action = action;
		}
		protected override string GetXmlCommand()
		{
			XElement root = new XElement("Message",
				new XAttribute("Title", _action.Title),
				new XAttribute("Message", _action.Message));


			string result = root.ToString();
			return result;
		}
	}
}