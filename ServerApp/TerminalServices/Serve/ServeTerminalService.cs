using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer;
using DataLayer.Data;
using ServerApp.Devices;
using ServerApp.Devices.Actions;
using ServerApp.TerminalServices.Serve.Layouts;
using ServerApp.TerminalServices.Serve.States;
using ServerApp.TerminalServices.Shared.States;

namespace ServerApp.TerminalServices.Serve
{
	public class ServeTerminalService : TerminalService, IServeTerminalService
	{
		public Rallo ClientDevice { get; private set; }
		public Rallo ServiceDevice { get; private set; }
		public ServeTerminalService(List<Device> devices, MyApplication app, DatabaseLayer dbLayer) : base(app, dbLayer)
		{
			if (devices.Count != 2)
				throw new ArgumentOutOfRangeException();
			// pripojenie sa na terminaly
			ClientDevice = new Rallo(this);
			ClientDevice.Connect(devices[0].IP, devices[0].Port);
			ServiceDevice = new Rallo(this);
			ServiceDevice.Connect(devices[1].IP, devices[1].Port);
		}

		// definicia layoutov ktore sa v automate budu pouzivat
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
