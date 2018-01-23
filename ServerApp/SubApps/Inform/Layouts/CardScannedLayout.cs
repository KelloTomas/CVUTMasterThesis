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

		public IEnumerable<ModifyLayoutItem> SetMeals(Client client)
		{
			yield return new ModifyLayoutItem("ClientName", "text", $"{client.FirstName} {client.LastName}");
			yield return new ModifyLayoutItem("AccountBalance", "text", $"{client.Balance} Kc");
			int i = 0;
			foreach (ServerApp.Order o in client.Orders)
			{
				if (o.Vydane)
					continue;
				if (i == 6)
					break;
				yield return new ModifyLayoutItem($"Date_{i}", "text", o.ForDate.ToString("dd.mm.yyyy"));
				yield return new ModifyLayoutItem($"IdMenu_{i}", "text", o.IdMenu.ToString());
				if (o.Menu.Soup == null)
					yield return new ModifyLayoutItem($"Soup_{i}", "text", "--");
				else
					yield return new ModifyLayoutItem($"Soup_{i}", "text", o.Menu.Soup.Name);
				if (o.Menu.Meal == null)
					yield return new ModifyLayoutItem($"Meal_{i}", "text", "--");
				else
					yield return new ModifyLayoutItem($"Meal_{i}", "text", o.Menu.Meal.Name);
				if (o.Menu.Desert == null)
					yield return new ModifyLayoutItem($"Desert_{i}", "text", "--");
				else
					yield return new ModifyLayoutItem($"Desert_{i}", "text", o.Menu.Desert.Name);
				i++;
			}
			while (i < 6)
			{
				yield return new ModifyLayoutItem($"Date_{i}", "text", " ");
				yield return new ModifyLayoutItem($"IdMenu_{i}", "text", "----");
				yield return new ModifyLayoutItem($"Soup_{i}", "text", " ");
				yield return new ModifyLayoutItem($"Meal_{i}", "text", " ");
				yield return new ModifyLayoutItem($"Desert_{i}", "text", " ");
				i++;
			}
		}
	}
}