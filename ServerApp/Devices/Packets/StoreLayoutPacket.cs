using System.Xml.Linq;

namespace ServerApp.Devices
{
	internal class StoreLayoutPacket : BasePacket
	{
		private IStoreLayoutAction _action;

		public StoreLayoutPacket(IStoreLayoutAction action)
		{
			this._action = action;
		}
		protected override string GetXmlCommand()
		{
			XElement root = new XElement("StoreLayout",
				new XAttribute("Id", _action.Id),
				_action.Layout);


			string result = root.ToString();
			return result;
		}
	}
}