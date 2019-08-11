using ImageAbridged.Models.Residence.Search.Listings;

namespace ImageAbridged.Models.Residence.InOut
{
	public class SearchApartmentInput : SearchLocationInput, IListingFilters
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
