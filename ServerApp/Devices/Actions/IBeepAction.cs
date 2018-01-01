namespace ServerApp.Devices
{
	internal interface IBeepAction
	{
		double Length { get; }
		double Delay { get; }
		int Count { get; }
	}
}