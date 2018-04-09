using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer;
using DataLayer.Data;
using ServerApp.Devices;
using ServerApp.Devices.Actions;
using ServerApp.SubApps.Serve.Layouts;
using ServerApp.SubApps.Serve.States;
using ServerApp.SubApps.Shared.States;

namespace ServerApp.SubApps.Serve
{
	public class ServeSubApp : SubApp, IServeSubApp
	{
		public Rallo ClientDevice { get; private set; }
		public Rallo ServiceDevice { get; private set; }
		public ServeSubApp(List<Device> devices, string appName, DatabaseLayer dbLayer) : base(appName, dbLayer)
		{
			if (devices.Count != 2)
				throw new ArgumentOutOfRangeException();
			ClientDevice = new Rallo(this);
			ClientDevice.Connect(devices[0].IP, devices[0].Port);
			ServiceDevice = new Rallo(this);
			ServiceDevice.Connect(devices[1].IP, devices[1].Port);
		}

		public ClientTextLayout ClientTextLayout { get; } = new ClientTextLayout();
		public ClientServedLayout ClientServedLayout { get; } = new ClientServedLayout();
		public SetServingLayout SetServingLayout { get; } = new SetServingLayout();
		public ServingLayout ServingLayout { get; } = new ServingLayout();
		public HistoryLayout HistoryLayout { get; } = new HistoryLayout();


		public override IStateBase GetInitState()
        {
            return new SetServing(this);
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
