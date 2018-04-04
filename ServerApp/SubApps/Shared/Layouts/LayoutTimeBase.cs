using ServerApp.SubApps.Shared.Data;
using System;
using System.Collections.Generic;

namespace ServerApp.SubApps.Shared.Layouts
{
	public class LayoutTimeBase : LayoutBase
	{
        public LayoutTimeBase() : base()
		{
        }
		public IEnumerable<ModifyLayoutItem> SetDateTimeTo()
		{
			return SetDateTimeTo(DateTime.Now);
		}
		public IEnumerable<ModifyLayoutItem> SetDateTimeTo(DateTime dateTime)
		{
			yield return new ModifyLayoutItem("DateValue", "text", dateTime.ToString("d"));
			yield return new ModifyLayoutItem("TimeValue", "text", dateTime.ToString("t"));
		}
	}
}
