﻿namespace ImageAbridged.Models.Residence.Search.Listings
{
	public class ListingFilters : IListingFilters
	{
		public int? MinRentAmount { get; set; }
		public int? MaxRentAmount { get; set; }
		public int? MinBeds { get; set; }
		public int? MaxBeds { get; set; }
		public int? MinBaths { get; set; }
		public int? PetFriendly { get; set; }
		public int? Style { get; set; }
		public int? Specialties { get; set; }
	}
}
