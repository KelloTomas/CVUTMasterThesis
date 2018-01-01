namespace ServerApp.Devices
{
	internal interface IShowMessageBoxAction
	{
		string Title { get; }
		string Message { get; }
	}
}