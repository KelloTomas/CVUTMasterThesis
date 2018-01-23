using System.Collections.Generic;

namespace ServerApp.Devices.Actions
{
	internal class StoreLayoutAction : IStoreLayoutAction
	{
		public StoreLayoutAction(string id, string layout)
		{
			Id = id;
			Layout = layout;
		}
		public string Layout { get; }
		public string Id { get; }

		public override string ToString()
		{
			return $"StoreLayout {Id} {Layout}";
		}
	}
}