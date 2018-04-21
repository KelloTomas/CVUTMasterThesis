using ServerApp.Devices.Actions;
using ServerApp.TerminalServices.Shared.Data;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ServerApp.Devices.Packets
{
	internal class ShowLayoutPacket : BasePacket
	{
		private readonly IShowLayoutAction _action;

		public ShowLayoutPacket(IShowLayoutAction action)
		{
			_action = action;
		}
		protected override string GetXmlCommand()
		{
			XElement root = new XElement("Show",
				GetShowContent());


			string result = root.ToString();
			return result;
		}

		private IEnumerable<object> GetShowContent()
		{
			yield return new XAttribute("Id", _action.LayoutName);
			foreach (ModifyLayoutItem modify in _action.ModifyLayoutItems)
			{
				yield return new XElement("ModifyAttribute",
					new XAttribute("Id", modify.Id),
					new XAttribute("Attribute", modify.AttributeName),
					new XAttribute("Value", modify.AttributeValue ?? string.Empty));
			}
		}
	}
}