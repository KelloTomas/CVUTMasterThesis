﻿using ServerApp.SubApps.Shared.Data;

namespace ServerApp.Devices.Actions
{
	internal interface IShowLayoutAction : IAction
	{
		string LayoutName { get; }
		ModifyLayoutItem[] ModifyLayoutItems { get; }
	}
}