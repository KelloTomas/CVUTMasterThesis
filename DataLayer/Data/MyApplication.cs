using System.Collections.Generic;

namespace DataLayer.Data
{
	public class MyApplication
	{
		public int Id { get; set; }
		public bool IsRunning { get; set; }
		public string TypeName { get; set; }
		public string AppName { get; set; }
        public ServingPlace ServingPlace { get; set; }
		public List<Device> Devices { get; set; }
	}
}
