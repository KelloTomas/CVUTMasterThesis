using ServerApp.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Data
{
	public class ButtonClickAction : IAction
	{
		public string ButtonName { get; }
	}
}
