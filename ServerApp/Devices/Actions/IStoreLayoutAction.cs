namespace ServerApp.Devices
{
	internal interface IStoreLayoutAction : IAction
	{
		string Id { get; }

		string Layout { get; }
	}
}