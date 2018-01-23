namespace ServerApp.Devices.Actions
{
	public interface IStoreLayoutAction : IAction
	{
		string Id { get; }

		string Layout { get; }
	}
}