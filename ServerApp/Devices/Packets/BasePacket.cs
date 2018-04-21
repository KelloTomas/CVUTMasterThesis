using System.Collections.Generic;
using System.Text;

namespace ServerApp.Devices.Packets
{
	public class BasePacket
	{
		private string _baseTag;

		public BasePacket(string baseTag = "RLI")
		{
			_baseTag = baseTag;
			Encoding = Encoding.GetEncoding("UTF-8");
		}

		public byte[] GetData()
		{
			List<byte> bytes = new List<byte>();

			byte[] prefix = Encoding.GetBytes($"<{_baseTag}>");
			bytes.AddRange(prefix);

			byte[] command = Encoding.GetBytes(GetXmlCommand());
			bytes.AddRange(command);

			byte[] suffix = Encoding.GetBytes($"</{_baseTag}>");
			bytes.AddRange(suffix);

			return bytes.ToArray();
		}

		public Encoding Encoding { get; }
		protected virtual string GetXmlCommand()
		{
			throw new System.Exception();
		}
	}
}