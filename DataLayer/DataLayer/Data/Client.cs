using System.Collections.Generic;

namespace DataLayer.Data
{
	public class Client
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName { get { return $"{LastName} {FirstName}"; } }
		public float Balance { get; set; }
		public string CardNumber { get; set; }
		public List<Order> Orders { get; set; }

		public override string ToString()
		{
			return $"{LastName} {FirstName}";
		}
	}
}