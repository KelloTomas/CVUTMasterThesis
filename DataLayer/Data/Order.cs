using System;

namespace DataLayer.Data
{
	public class Order : Menu
	{
		public int IdOrder { get; set; }
		public Client Client { get; set; }
		public DateTime? Served { get; set; }
	}
}