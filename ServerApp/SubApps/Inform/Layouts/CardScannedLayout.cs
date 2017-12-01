using ServerApp.SubApps.Shared;
using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using System.Collections.Generic;

namespace ServerApp.SubApps.Inform.Layouts
{
	/// <summary>
	/// Obrazovka pre klienta, pouziva sa pri necinnosti s textom neprikladajte kartu, pripadne ak je prilozena neznama karta
	/// </summary>
	public class CardScannedLayout : LayoutTimeBase
	{
		#region private fields...
		private const int _screenMealCount = 4;
        #endregion

        #region constructors...
        public CardScannedLayout() : base()
		{
        }
		#endregion


	} }