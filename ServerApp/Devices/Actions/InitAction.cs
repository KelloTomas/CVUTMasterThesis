using ServerApp.Data;

namespace ServerApp.Devices
{
	internal class InitAction : IInitAction
	{
		private object p;

		public InitAction()
		{
		}

		public override string ToString()
		{
			return "Init";
		}
	}
}