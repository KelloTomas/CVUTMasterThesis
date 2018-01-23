using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Globalization;
using ServerApp.Devices.Actions;

namespace ServerApp.Devices
{
	public class QtPacketParser
	{
		public IEnumerable<IAction> CreateAction(string message)
		{
			using (XmlReader xmlReader = XmlReader.Create(new StringReader(message)))
			{
				if (xmlReader.ReadToFollowing("RLO"))
				{
					while (xmlReader.Read())
					{
						if (xmlReader.NodeType == XmlNodeType.Element)
						{
							DateTime timeStamp = DateTime.Parse(xmlReader.GetAttribute("TimeStamp"), CultureInfo.InvariantCulture);
							if (xmlReader.Name == "ButtonClick")
							{
								string id = xmlReader.GetAttribute("Id");
								yield return new ButtonClickAction(id, timeStamp);
							}
							else if (xmlReader.Name == "CardRead")
							{
								string number = xmlReader.GetAttribute("Number");
								yield return new CardReadAction(number, timeStamp);
							}
							else if (xmlReader.Name == "Error")
							{
								string erroMsg = xmlReader.GetAttribute("Message");
							}
							else
							{
							}
						}
					}
				}
			}
		}
	}
}
