using DataLayer.Data;
using ServerApp.TerminalServices.Shared.Data;
using ServerApp.TerminalServices.Shared.Layouts;
using System;
using System.Collections.Generic;

namespace ServerApp.TerminalServices.Serve.Layouts
{
	/// <summary>
	/// Obrazovka z pohladu klienta pri zobrazeni jedla pripadne ak nema objednane jedlo ale ma dalsie objednavky
	/// </summary>
	public class ServingLayout : LayoutTimeBase
	{
		#region constructors...
		public ServingLayout() : base()
		{
		}
		#endregion

		#region public class...
		public static class Buttons
		{
			public const string StopBtn = "StopBtn";
			public const string ShowServedBtn = "ShowServedBtn";
		}
		public static class Labels
		{
			public static string Title = "TitleValue";
			public static string Id(int id) { return $"Id{id}"; }
			public static string Name(int id) { return $"Name{id}"; }
			public static string Description(int id) { return $"Desc{id}"; }
		}
		#endregion

		#region public methods...
		public IEnumerable<ModifyLayoutItem> SetMenu(Menu menu, Client client, string title)
		{
			if (client == null)
			{
				yield return new ModifyLayoutItem(Labels.Title, "text", string.IsNullOrWhiteSpace(title) ? " " : title);
			}
			else
			{
				yield return new ModifyLayoutItem(Labels.Title, "text", client.FullName);
			}
			if (menu == null)
			{
				for (int i = 0; i < 3; i++)
				{
					yield return new ModifyLayoutItem(Labels.Id(i), "text", "-");
					yield return new ModifyLayoutItem(Labels.Name(i), "text", "---");
					yield return new ModifyLayoutItem(Labels.Description(i), "text", "---");
				}
			}
			else
			{
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
		}
		#endregion
	}
}
