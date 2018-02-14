using DataLayer.Data;
using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using System.Collections.Generic;

namespace ServerApp.SubApps.Order.Layouts
{
	/// <summary>
	/// Obrazovka pre klienta, pouziva sa pri necinnosti s textom neprikladajte kartu, pripadne ak je prilozena neznama karta
	/// </summary>
	public class MessageLayout : LayoutTimeBase
	{

		#region constructors...
		public MessageLayout() : base()
		{
		}
		#endregion

		public IEnumerable<ModifyLayoutItem> SetText(Client client, string clientMsg)
		{
			if (client == null)
			{
				yield return new ModifyLayoutItem("ClientName", "text", $"--");
				yield return new ModifyLayoutItem("AccountBalance", "text", $" ");
			}
			else
			{
				yield return new ModifyLayoutItem("ClientName", "text", $"{client.LastName} {client.FirstName}");
				yield return new ModifyLayoutItem("AccountBalance", "text", $"{client.Balance} Kc");
			}
			yield return new ModifyLayoutItem($"contentValue", "text", clientMsg);
		}
	}
}