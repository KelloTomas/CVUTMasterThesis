namespace DataLayer.Data
{
	public class MenuItem
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public override string ToString()
		{
            if (string.IsNullOrWhiteSpace(Description))
                return Name;
			return $"{Name}: {Description}";
		}
	}
}
