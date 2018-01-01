using System;
using System.Xml.Linq;

namespace ServerApp.Devices
{
	internal class InitPacket : BasePacket
	{
		public InitPacket()
		{
		}
		protected override string GetXmlCommand()
		{
			XElement root = new XElement("Init",
				new XAttribute("CurrentTime", DateTime.Now.ToUniversalTime().ToString("MMddHHmmyyyy.ss")));

			string result = root.ToString();
			return result;
		}
	}
}