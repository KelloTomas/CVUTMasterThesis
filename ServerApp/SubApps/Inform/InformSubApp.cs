using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using ServerApp.SubApps.Shared.States;
using ServerApp.SubApps.Inform.States;
using ServerApp.Devices;
using System.Linq;
using ServerApp.SubApps.Inform.Layouts;
using ServerApp.Devices.Actions;

namespace ServerApp.SubApps.Inform
{
	public class InformSubApp : SubApp, IInformSubApp
	{
        public Rallo Rallo {get; private set;}

		public InformSubApp(List<Device> devices) : base()
		{
            if (devices.Count != 1)
                throw new ArgumentOutOfRangeException();
            Rallo = new Rallo(this);
			//rallo.Connect(devices[0].IP, devices[0].Port ?? 15000);
			Rallo.Connect("127.0.0.1", 15000);
		}

		public CardScannedLayout CardScannedLayout { get; } = new CardScannedLayout();
		public ScanCardLayout ScanCardLayout { get; } = new ScanCardLayout();
		public override void Init()
		{
		}
		public override IStateBase GetInitState()
        {
            return new InformState(this, 10000);
        }
		public override IEnumerable<IStoreLayoutAction> GetStoreLayoutActions()
		{
			System.Reflection.PropertyInfo[] properties = GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance);
			foreach (System.Reflection.PropertyInfo propertyInfo in properties.Where(p => typeof(Shared.Layouts.LayoutBase).IsAssignableFrom(p.PropertyType)))
			{
				Shared.Layouts.LayoutBase layout = (Shared.Layouts.LayoutBase)propertyInfo.GetValue(this, null);
				yield return new StoreLayoutAction(layout.Name, layout.GetLayout());
			}
		}
	}
}
