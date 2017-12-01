using Anete.Common.Data.Business.Authentication;
using Anete.Common.Data.Entities;
using Anete.Config.Core;
using Anete.Ctecky.Automat.Qt.Informacni.Layouts;
using Anete.Ctecky.Automat.Qt.Informacni.States;
using Anete.Ctecky.Automat.Qt.Shared;
using Anete.Ctecky.DBConnection.Services.CardAuthentication;
using Anete.Ctecky.DBConnection.Services.OrdersInfo;
using Anete.Ctecky.Shared;
using Anete.Ctecky.Shared.Actions.Qt;
using Anete.Ctecky.Shared.Automat;
using Anete.Ctecky.Shared.Configuration;
using Anete.Ctecky.Shared.InstanceAttributes;
using Anete.Ctecky.Shared.Messages;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Anete.Ctecky.Automat.Qt.Informacni
{
	[AutomatHelpAttribute("Informační Qt", "Informační automat pro Linuxový terminál s raspberry pi použivající uživatelský interface Qt", "Informační", new int[] { 202, 802 })]
	[AutomatCreateInstance]
	public class InformacniQt : QtAutomatBase, IInformacniQt
	{
		public InformacniQt(IZarizeni zarizeni, short id, IIDGenerator idGenerator, string logDirectory, IAuthenticationDataLayer authenticationDataLayer, IConfigManager configManager,
			IOrdersInfoService ordersInfoService, CanteenProvider canteenRepository, ScanCardLayout scanCardLayout,
			CardScannedLayout cardScannedLayout, TimerFactory timerFactory) : base(zarizeni, id, idGenerator, logDirectory, canteenRepository, timerFactory)
		{
			// musim predat Logger pochazeji primo z automatu, abych mel spravne zaznamenany SQL dotazy
			Log4Net.Core.ILogEx dbLog = this.GetDbLog();

			OrdersInfoService = ordersInfoService;
			ordersInfoService.Initialize(dbLog);

			AuthenticationDataLayer = authenticationDataLayer;
			AuthenticationDataLayer.InitLog(dbLog);

			Typ = 202;

			CardScannedLayout = cardScannedLayout;
			ScanCardLayout = scanCardLayout;
		}

		[Browsable(false)]
		public IAuthenticationDataLayer AuthenticationDataLayer { get; }
		//internal ICardAuthenticationService CardAuthenticationService { get; }
		internal IOrdersInfoService OrdersInfoService { get; }

		[Browsable(false)]
		public CardScannedLayout CardScannedLayout { get; }
		[Browsable(false)]
		public ScanCardLayout ScanCardLayout { get; }
		public override Thread Init()
		{
			IdleState = new Inform(this);
			return base.Init();
		}


		[TimeOut()]
		[DefaultValue(typeof(int), "10000")]
		[DBStored]
		[Description("Dlzka v ms, ako dlho bude po prilozeni karty zobrazena sprava (objednane jedla, neznama karta, systemova karta)")]
		public int MessageShowLength { get; set; } = 10000;

		[TimeOut(false)]
		[DefaultValue(typeof(int), "1500")]
		[DBStored]
		[Description("Dlzka v ms, po akej dobe sa povoli dalsie prilozenie karty")]
		public int CardScanSavePeriod { get; set; } = 1500;


		protected override IEnumerable<IStoreRefQtAction> GetStoreRefActions()
		{
			yield break;
		}
	}
}
