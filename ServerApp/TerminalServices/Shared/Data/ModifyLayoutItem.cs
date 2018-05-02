using ServerApp.Devices.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerApp.TerminalServices.Shared.Data
{
	// funkcia na upravu objektu na obrazovke
	public class ModifyLayoutItem
	{
		public ModifyLayoutItem(string id, string attributeName, string attributeValue)
		{
			AttributeValue = attributeValue;
			AttributeName = attributeName;
			Id = id;
		}
		public string Id { get; }
		public string AttributeName { get; }
		public string AttributeValue { get; }

		public override string ToString()
		{
			return $"ModifyLayout {Id} {AttributeName}={AttributeValue}";
		}
	}
}
