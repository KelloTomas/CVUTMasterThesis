using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Data
{
	public class MyApplication
	{
		public bool IsRunning { get; set; }
		public string Name { get; set; }
		public List<Device> Devices { get; set; }
	}
}
