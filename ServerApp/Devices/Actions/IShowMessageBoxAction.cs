namespace ServerApp.Devices.Actions
{
	internal interface IShowMessageBoxAction
	{
		string Title { get; }
		string Message { get; }
	}
}