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
	public class SetServingLayout : LayoutTimeBase
	{
		#region constructors...
		public SetServingLayout() : base()
		{
		}
		#endregion

		#region public class...
		public static class Buttons
		{
			public const string StartBtn = "startBtn";
			public const string PrevBtn = "PrevBtn";
			public const string NextBtn = "NextBtn";
		}
		public static class Labels
		{
			public static string Title = "TitleValue";
			public static string MealId = "IdValue";
			public static string SoupName = "SoupValue";
			public static string MealName = "MealValue";
			public static string DesertName = "DezertValue";
		}
		#endregion

		#region public methods...
		public IEnumerable<ModifyLayoutItem> SetTitle(string title)
		{
			yield return new ModifyLayoutItem(Labels.Title, "text", title);
		}
		public IEnumerable<ModifyLayoutItem> SetMeal(Menu meal)
		{
			if (meal == null)
			{
				yield return new ModifyLayoutItem(Labels.MealId, "text", "--");
				yield return new ModifyLayoutItem(Labels.SoupName, "text", " ");
				yield return new ModifyLayoutItem(Labels.MealName, "text", " ");
				yield return new ModifyLayoutItem(Labels.DesertName, "text", " ");
			}
			else
			{
				yield return new ModifyLayoutItem(Labels.MealId, "text", meal.IdMenu.ToString());
				yield return new ModifyLayoutItem(Labels.SoupName, "text", meal.Items[0] == null ? " " : meal.Items[0].Name);
				yield return new ModifyLayoutItem(Labels.MealName, "text", meal.Items[1] == null ? " " : meal.Items[1].Name);
				yield return new ModifyLayoutItem(Labels.DesertName, "text", meal.Items[2] == null ? " " : meal.Items[2].Name);
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
			if (page >= pageCount-1)
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
