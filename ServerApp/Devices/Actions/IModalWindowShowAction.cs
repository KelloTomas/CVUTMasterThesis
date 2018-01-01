namespace ServerApp.Devices
{
	internal interface IModalWindowShowAction
	{
		string Message { get; }
		ModalWindowButton[] Buttons { get; }
	}
}