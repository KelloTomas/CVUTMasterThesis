using DataLayer.Data;
using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using System;
using System.Collections.Generic;

namespace ServerApp.SubApps.Order.Layouts
{
	/// <summary>
	/// Obrazovka z pohladu klienta pri zobrazeni jedla pripadne ak nema objednane jedlo ale ma dalsie objednavky
	/// </summary>
	public class OrdersLayout : LayoutTimeBase
	{
		public int MEALS_PER_PAGE = 4;
		#region constructors...
		public OrdersLayout() : base()
		{
		}
		#endregion

		#region public methods...
		public IEnumerable<ModifyLayoutItem> SetDate(DateTime date, bool isOrderForNextDay)
		{
			yield return new ModifyLayoutItem("toDate", "text", date.ToString("dd/MM/yyyy"));
            yield return new ModifyLayoutItem("nextDay", "enabled", isOrderForNextDay.ToString());
            bool isPrev = date > DateTime.Now.Date;
            yield return new ModifyLayoutItem("prevDay", "enabled", isPrev.ToString());
        }
		public IEnumerable<ModifyLayoutItem> SetMeals(List<Menu> menu, int pageNum, int? selected)
		{
			if (pageNum == 0)
				yield return new ModifyLayoutItem($"up", "enabled", "false");
			else
				yield return new ModifyLayoutItem($"up", "enabled", "true");
			if ((pageNum+1)*MEALS_PER_PAGE <= menu.Count )
				yield return new ModifyLayoutItem($"down", "enabled", "true");
			else
				yield return new ModifyLayoutItem($"down", "enabled", "false");
			int index;
			for (int i = 0; i < MEALS_PER_PAGE; i++)
			{
				index = pageNum * MEALS_PER_PAGE + i;
				if (menu.Count <= index)
				{
					yield return new ModifyLayoutItem($"menu_{i}", "text", "--");
					yield return new ModifyLayoutItem($"menu_{i}", "enabled", "false");
                    yield return new ModifyLayoutItem($"menu_{i}", "checked", "false");
                }
				else
				{
					yield return new ModifyLayoutItem($"menu_{i}", "text", $"{menu[index].IdMenu}: {menu[index].SoupName}, {menu[index].MealName}, {menu[index].DesertName}");
					    yield return new ModifyLayoutItem($"menu_{i}", "enabled", "true");
				    if (index == selected)
					    yield return new ModifyLayoutItem($"menu_{i}", "checked", "true");
				    else
					    yield return new ModifyLayoutItem($"menu_{i}", "checked", "false");
				}

			}
		}
		#endregion
	}
}
