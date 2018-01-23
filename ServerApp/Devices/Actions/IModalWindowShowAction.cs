namespace ServerApp.Devices.Actions
{
	internal interface IModalWindowShowAction
	{
		string Message { get; }
		ModalWindowButton[] Buttons { get; }
	}
}