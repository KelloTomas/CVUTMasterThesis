using DataLayer.Data;
using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using System;
using System.Collections.Generic;

namespace ServerApp.SubApps.Serve.Layouts
{
	/// <summary>
	/// Obrazovka z pohladu klienta pri zobrazeni jedla pripadne ak nema objednane jedlo ale ma dalsie objednavky
	/// </summary>
	public class ClientServedLayout : LayoutTimeBase
	{
		#region constructors...
		public ClientServedLayout() : base()
		{
		}
		#endregion

		#region public class...
		public static class Labels
		{
			public static string Title = "TitleValue";
			public static string Balance = "BalanceValue";
			public static string Id(int id) { return $"Id{id}"; }
			public static string Name(int id) { return $"Name{id}"; }
			public static string Description(int id) { return $"Desc{id}"; }
		}
		#endregion

		#region public methods...
		public IEnumerable<ModifyLayoutItem> SetTexts(string title)
		{
			yield return new ModifyLayoutItem("TitleValue", "text", title);
		}
		public IEnumerable<ModifyLayoutItem> SetContent(string contenttext)
		{
			yield return new ModifyLayoutItem("content", "text", contenttext);
		}



		public IEnumerable<ModifyLayoutItem> SetMenu(Menu menu, Client client, string title)
		{
			yield return new ModifyLayoutItem(Labels.Title, "text", client.FullName);
			yield return new ModifyLayoutItem(Labels.Balance, "text", client.Balance.ToString("F2"));
			int i = 0;
			foreach (var item in menu.Items)
			{
				if (item == null)
				{
					yield return new ModifyLayoutItem(Labels.Id(i), "text", " ");
					yield return new ModifyLayoutItem(Labels.Name(i), "text", " ");
					yield return new ModifyLayoutItem(Labels.Description(i), "text", " ");
				}
				else
				{
					yield return new ModifyLayoutItem(Labels.Id(i), "text", item.Id.ToString());
					yield return new ModifyLayoutItem(Labels.Name(i), "text", item.Name);
					yield return new ModifyLayoutItem(Labels.Description(i), "text", item.Description);
				}
				i++;
			}
		}
		#endregion
	}
}
