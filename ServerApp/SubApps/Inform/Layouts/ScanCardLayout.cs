using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using System.Collections.Generic;

namespace ServerApp.SubApps.Inform.Layouts
{
	/// <summary>
	/// Obrazovka z pohladu klienta pri zobrazeni jedla pripadne ak nema objednane jedlo ale ma dalsie objednavky
	/// </summary>
	public class ScanCardLayout : LayoutTimeBase
	{
        #region constructors...
        public ScanCardLayout() : base()
		{
        }
		#endregion

		#region public class...
		public static class Layouts
		{
			public static class Title
			{
				public static string Name = "titleLayout";
				public static string ShowAccountBalance(bool show)
				{
					return show ? "4,2,1" : "6,0,1";
				}
			}
		}
		public static class Labels
		{
			public static string TitleValue = "TitleValue";
			public static string ContentValue = "ContentValue";
			public static string AccountBalance = "AccountBalance";
		}
		#endregion

		#region public methods...
		public IEnumerable<ModifyLayoutItem> SetTexts(string pageTitle, decimal? accountBalance, string message)
		{
			yield return new ModifyLayoutItem(Labels.TitleValue, "text", pageTitle);
			if (accountBalance == null)
			{
				yield return new ModifyLayoutItem(Layouts.Title.Name, "stretch", Layouts.Title.ShowAccountBalance(false));
			}
			else
			{
				yield return new ModifyLayoutItem(Layouts.Title.Name, "stretch", Layouts.Title.ShowAccountBalance(true));
				yield return new ModifyLayoutItem(Labels.AccountBalance, "text", "85");
			}
			yield return new ModifyLayoutItem(Labels.ContentValue, "text", message);
		}
		#endregion
	}
}
