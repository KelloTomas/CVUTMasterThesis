using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using System.Collections.Generic;

namespace ServerApp.SubApps.Serve.Layouts
{
	/// <summary>
	/// Obrazovka z pohladu klienta pri zobrazeni jedla pripadne ak nema objednane jedlo ale ma dalsie objednavky
	/// </summary>
	public class HistoryLayout : LayoutTimeBase
	{
		#region public class...
		public static class Buttons
		{
			public const string PrevBtn = "PrevBtn";
			public const string NextBtn = "NextBtn";
			public const string BackBtn = "BackBtn";
		}
		public static class Labels
		{
			public static string UserName = "UserValue";
			public static string ServedTime = "ServedTimeValue";
			public static string Id(int id) { return $"Id{id}"; }
			public static string Name(int id) { return $"Name{id}"; }
			public static string Description(int id) { return $"Desc{id}"; }
		}
		#endregion

		#region public methods...
		public IEnumerable<Shared.Data.ModifyLayoutItem> SetMenu(DataLayer.Data.Order order)
		{
			if (order == null)
			{
				yield return new ModifyLayoutItem(Labels.UserName, "text", "--");
				yield return new ModifyLayoutItem(Labels.ServedTime, "text", " ");

				for (int i = 0; i < 3; i++)
				{
					yield return new ModifyLayoutItem(Labels.Id(i), "text", "-");
					yield return new ModifyLayoutItem(Labels.Name(i), "text", "---");
					yield return new ModifyLayoutItem(Labels.Description(i), "text", "---");
				}
			}
			else
			{
				yield return new ModifyLayoutItem(Labels.UserName, "text", order.Client.FullName);
				yield return new ModifyLayoutItem(Labels.ServedTime, "text", order.Served.ToString());
				int i = 0;
				foreach (var item in order.Items)
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

		public IEnumerable<ModifyLayoutItem> SetNavigationButtons(int page, int pageCount)
		{
			if (page == 0)
			{
				yield return new ModifyLayoutItem(Buttons.PrevBtn, "enabled", false.ToString());
			}
			else
			{
				yield return new ModifyLayoutItem(Buttons.PrevBtn, "enabled", true.ToString());
			}
			if (page >= pageCount - 1)
			{
				yield return new ModifyLayoutItem(Buttons.NextBtn, "enabled", false.ToString());
			}
			else
			{
				yield return new ModifyLayoutItem(Buttons.NextBtn, "enabled", true.ToString());
			}
		}
		#endregion
	}
}
