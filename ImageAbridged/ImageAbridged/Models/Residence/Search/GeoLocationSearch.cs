namespace ImageAbridged.Models.Residence.Search
{
	public class GeoLocationSearch
	{
		public string ID { get; set; }
		public string Display { get; set; }
		public int GeographyType { get; set; }
		public Address Address { get; set; }
		public Location Location { get; set; }
		public BoundingBox BoundingBox { get; set; }
		public int Radius { get; set; }
	}
}
