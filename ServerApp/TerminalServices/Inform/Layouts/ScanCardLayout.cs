using ServerApp.TerminalServices.Shared.Data;
using ServerApp.TerminalServices.Shared.Layouts;
using System.Collections.Generic;

namespace ServerApp.TerminalServices.Inform.Layouts
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

		#region public methods...
		public IEnumerable<ModifyLayoutItem> SetTexts(string pageTitle, string message)
		{
			yield return new ModifyLayoutItem("TitleValue", "text", pageTitle);
			yield return new ModifyLayoutItem("ContentValue", "text", message);
		}
		#endregion
	}
}
