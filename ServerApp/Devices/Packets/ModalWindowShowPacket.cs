using System.Collections.Generic;
using System.Xml.Linq;

namespace ServerApp.Devices
{
	internal class ModalWindowShowPacket : BasePacket
	{
		private readonly IModalWindowShowAction _action;

		public ModalWindowShowPacket(IModalWindowShowAction action)
		{
			_action = action;
		}

		protected override string GetXmlCommand()
		{
			XElement root = new XElement("Modal",
				GetButtons());
			string result = root.ToString();
			return result;
		}

		private IEnumerable<object> GetButtons()
		{
			yield return new XAttribute("Text", _action.Message);
			int i = 0;
			foreach (ModalWindowButton button in _action.Buttons)
			{
				yield return new XElement("Button",
					new XAttribute("Id", button.BtnId),
					new XAttribute("Text", button.BtnText));
				i++;
			}
		}
	}
}