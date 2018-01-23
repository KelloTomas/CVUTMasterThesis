using System.Xml.Linq;

namespace ServerApp.Devices.Packets
{
	internal class ModalWindowClosePacket : BasePacket
	{
		public ModalWindowClosePacket()
		{
		}
		protected override string GetXmlCommand()
		{
			XElement root = new XElement("ModalClose");

			string result = root.ToString();
			return result;
		}
	}
}