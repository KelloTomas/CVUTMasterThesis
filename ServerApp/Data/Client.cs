using System.Collections.Generic;

namespace ServerApp.Data
{
	public class Client
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public double Balance { get; set; }
		public List<Menu> Orders { get; set; }
	}
}