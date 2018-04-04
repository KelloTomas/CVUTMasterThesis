using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;
using ServerApp.SubApps.Shared.States;
using ServerApp.Devices;
using ServerApp.SubApps.Shared.Data;
using ServerApp.SubApps.Shared.Layouts;
using ServerApp.Devices.Actions;
using System.Data.Entity;
using DataLayer.Data;

namespace ServerApp.SubApps.Serve.States
{
	public class Serving : StateBase
	{

        #region private fields...
        List<Menu> menuOnScreen;
        ServeSubApp app;
		LayoutBase layout;
		Client client;
		DateTime dateToOrder;
		private string clientMsg = null;
		IEnumerable<Menu> menu;
		private int pageNum;
		private int? selected;
        bool reset = true;
		#endregion

		#region constructors...
		public Serving(ServeSubApp subApp) : base(3000)
		{
			app = subApp;
		}
		#endregion
		public override void Enter()
        {
            base.Enter();
			menu = app.databaseLayer.GetMenu();
            pageNum = 0;
        }

        public override IStateBase ProcessTimerElapsed()
		{
			return base.ProcessTimerElapsed();
		}
	}
}