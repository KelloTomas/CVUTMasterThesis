namespace ServerApp.Devices
{
	public enum ConnectionStatusEnum
	{
		/// <summary>
		/// Třída není k HW připojena
		/// </summary>
		Disconnected,
		/// <summary>
		/// Právě probíhá připojování
		/// </summary>
		Connecting,
		/// <summary>
		/// Došlo k navázání připojení, ale ještě nebyla provedena inicializace HW
		/// </summary>
		ConnectedNotInicialized,
		/// <summary>
		/// Spojení a inicializace byly úspěšné
		/// </summary>
		ConnectedInicialized
	}
}