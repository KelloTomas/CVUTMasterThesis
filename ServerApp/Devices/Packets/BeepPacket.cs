using System.Collections.Generic;
using System.Xml.Linq;

namespace ServerApp.Devices
{
	internal class BeepPacket : BasePacket
	{
		private readonly IBeepAction _action;

		public BeepPacket(IBeepAction action)
		{
			_action = action;
		}
		protected override string GetXmlCommand()
		{
			XElement root = new XElement("Beep",
				GetShowContent());


			string result = root.ToString();
			return result;
		}

		private IEnumerable<object> GetShowContent()
		{
			yield return new XAttribute("Length", _action.Length);
			yield return new XAttribute("Delay", _action.Delay);
			yield return new XAttribute("Count", _action.Count);
		}
	}
}