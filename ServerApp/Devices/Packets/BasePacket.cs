using System.Collections.Generic;
using System.Text;

namespace ServerApp.Devices
{
	public class BasePacket
	{
		private string BaseTag;

		public BasePacket(string baseTag = "RSI")
		{
			BaseTag = baseTag;
			Encoding = Encoding.GetEncoding("UTF-8");
		}

		public byte[] GetData()
		{
			List<byte> bytes = new List<byte>();

			byte[] prefix = Encoding.GetBytes($"<{BaseTag}>");
			bytes.AddRange(prefix);

			// vlastni kod
			byte[] command = Encoding.GetBytes(GetXmlCommand());
			bytes.AddRange(command);

			byte[] suffix = Encoding.GetBytes($"</{BaseTag}>");
			bytes.AddRange(suffix);

			string msg = UnicodeEncoding.ASCII.GetString(bytes.ToArray());

			return bytes.ToArray();
		}

		public Encoding Encoding { get; }
		protected virtual string GetXmlCommand()
		{
			throw new System.Exception();
		}
	}
}