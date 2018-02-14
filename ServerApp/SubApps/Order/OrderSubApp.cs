using System;
using System.Collections.Generic;
using ServerApp.SubApps.Shared.States;
using ServerApp.Devices;
using ServerApp.SubApps.Order.Layouts;
using ServerApp.SubApps.Order.States;
using ServerApp.Devices.Actions;
using System.Linq;
using DataLayer;
using DataLayer.Data;

namespace ServerApp.SubApps.Order
{
	public class OrderSubApp : SubApp, IOrderSubApp
	{
		public Rallo Rallo { get; private set; }
		public OrderSubApp(List<Device> devices, DatabaseLayer dbLayer) : base(dbLayer)
		{
			if (devices.Count != 1)
				throw new ArgumentOutOfRangeException();
			Rallo = new Rallo(this);
			Rallo.Connect(devices[0].IP, devices[0].Port);
		}
		public OrdersLayout OrdersLayout { get; } = new OrdersLayout();
		public MessageLayout MessageLayout { get; } = new MessageLayout();
		public override IStateBase GetInitState()
        {
            return new OrderState(this);
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
