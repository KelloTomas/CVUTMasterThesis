using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using System.Collections.Generic;

namespace ServerApp.SubApps.Inform.Layouts
{
	/// <summary>
	/// Obrazovka pre klienta, pouziva sa pri necinnosti s textom neprikladajte kartu, pripadne ak je prilozena neznama karta
	/// </summary>
	public class CardScannedLayout : LayoutTimeBase
	{

		#region constructors...
		public CardScannedLayout() : base()
		{
		}
		#endregion

		public IEnumerable<ModifyLayoutItem> SetMeals(Data.Client client)
		{
			yield return new ModifyLayoutItem("ClientName", "text", $"{client.FirstName} {client.LastName}");
			yield return new ModifyLayoutItem("AccountBalance", "text", $"{client.Balance} Kc");
			for (int i = 0; i <= 6; i++)
			{
				if (i < client.Orders.Count)
				{
					yield return new ModifyLayoutItem($"Date_{i}", "text", client.Orders[i].ForDate.ToString("dd.MM.yyyy"));
					yield return new ModifyLayoutItem($"IdMenu_{i}", "text", client.Orders[i].IdMenu.ToString());
					yield return new ModifyLayoutItem($"Soup_{i}", "text", client.Orders[i].SoupName);
					yield return new ModifyLayoutItem($"Meal_{i}", "text", client.Orders[i].MealName);
					yield return new ModifyLayoutItem($"Desert_{i}", "text", client.Orders[i].DesertName);
				}
				else
				{
					yield return new ModifyLayoutItem($"Date_{i}", "text", " ");
					yield return new ModifyLayoutItem($"IdMenu_{i}", "text", "----");
					yield return new ModifyLayoutItem($"Soup_{i}", "text", " ");
					yield return new ModifyLayoutItem($"Meal_{i}", "text", " ");
					yield return new ModifyLayoutItem($"Desert_{i}", "text", " ");
				}

			}
		}
	}
}