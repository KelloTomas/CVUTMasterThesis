namespace DataLayer.Data
{
	public class Order : Menu
	{
		public int IdClient { get; set; }
		public int IdOrder { get; set; }
		public bool Served { get; set; }
	}
}