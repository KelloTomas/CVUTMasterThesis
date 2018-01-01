using ServerApp.SubApps.Shared.Data;

namespace ServerApp.Devices
{
	internal interface IShowLayoutAction : IAction
	{
		string LayoutName { get; }
		ModifyLayoutItem[] ModifyLayoutItems { get; }
	}
}