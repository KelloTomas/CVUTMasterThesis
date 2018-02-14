using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Data
{
	public class MyApplication
	{
		public int Id { get; set; }
		public bool IsRunning { get; set; }
		public string TypeName { get; set; }
		public string AppName { get; set; }
		public List<Device> Devices { get; set; }
	}
}
