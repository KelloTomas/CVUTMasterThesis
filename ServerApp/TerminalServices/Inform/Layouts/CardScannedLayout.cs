using ServerApp.TerminalServices.Shared.Data;
using ServerApp.TerminalServices.Shared.Layouts;
using System.Collections.Generic;
using DataLayer.Data;

namespace ServerApp.TerminalServices.Inform.Layouts
{
	// obrazovka so zobrazenymi jedlami
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
			for (int i = 0; i <= 6; i++)
			{
				if (i < client.Orders.Count)
				{
					yield return new ModifyLayoutItem($"Date_{i}", "text", client.Orders[i].ForDate.ToString("dd.MM.yyyy"));
					yield return new ModifyLayoutItem($"IdMenu_{i}", "text", client.Orders[i].IdMenu.ToString());
					yield return new ModifyLayoutItem($"Soup_{i}", "text", client.Orders[i].Items[0]?.Name);
					yield return new ModifyLayoutItem($"Meal_{i}", "text", client.Orders[i].Items[1]?.Name);
					yield return new ModifyLayoutItem($"Desert_{i}", "text", client.Orders[i].Items[2]?.Name);
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