namespace ServerApp.Devices.Actions
{

	public static class ModalWindow
	{
		public static string ButtonPrefix = "ModalBtn_";
	}
	public class ModalWindowButton
	{
		public string BtnText { get; set; }
		public string BtnId { get; set; }
	}
	internal class ModalWindowShowAction : IAction, IModalWindowShowAction
	{
		public string Message { get; }
		public ModalWindowButton[] Buttons { get; }

		public ModalWindowShowAction(string message, ModalWindowButton[] buttonsText)
		{
			Message = message;
			Buttons = buttonsText;
		}

		public override string ToString()
		{
			return $"Show modal window, text: {Message}, with {Buttons.Length} buttons";
		}
	}
}