using ServerApp.SubApps.Shared.Data;

namespace ServerApp.Devices
{
	public class ShowLayoutAction : IShowLayoutAction
	{
		public ShowLayoutAction(string layoutName, params ModifyLayoutItem[] modifyLayoutItems)
		{
			ModifyLayoutItems = modifyLayoutItems;
			LayoutName = layoutName;
		}
		public string LayoutName { get; }
		public ModifyLayoutItem[] ModifyLayoutItems { get; }

		public override string ToString()
		{
			return $"ShowLayout {LayoutName} ({ModifyLayoutItems.Length} modifiers)";
		}
	}
}