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
	public class ClientTextLayout : LayoutTimeBase
	{
		#region constructors...
		public ClientTextLayout() : base()
		{
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
		#endregion
	}
}
