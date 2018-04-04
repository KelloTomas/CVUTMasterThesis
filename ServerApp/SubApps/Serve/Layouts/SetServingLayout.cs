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

		#region public methods...		
		public IEnumerable<ModifyLayoutItem> SetTexts(string title, string start)
		{
			yield return new ModifyLayoutItem("TitleValue", "text", title);
			yield return new ModifyLayoutItem("startBtn", "text", start);
		}
		public IEnumerable<ModifyLayoutItem> SetButtonsTexts(List<string> buttonsTitle)
		{
			int i = 0;
			foreach (string buttonTitle in buttonsTitle)
			{
				yield return new ModifyLayoutItem($"btn_{i}", "text", buttonTitle);
			}
		}
		#endregion
	}
}
