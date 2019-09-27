using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAbridged.Models.Residence.Search.Listings
{
	public class Placard
	{
		public string Title { get; set; }
		public string Location { get; set; }
		public List<string> ImageUrls { get; set; }
		public string RentRange { get; set; }
		public string DetailPageUrl { get; set; }
		public string Phone { get; set; }
		public string ResidentUnit { get; set; }
		public string Availability { get; set; }
	}
}
