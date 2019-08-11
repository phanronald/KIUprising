using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAbridged.Models.Residence.Search.Listings
{
	public interface IListingFilters
	{
		int? MinRentAmount { get; set; }
		int? MaxRentAmount { get; set; }
		int? MinBeds { get; set; }
		int? MaxBeds { get; set; }
		int? MinBaths { get; set; }
		int? PetFriendly { get; set; }
		int? Style { get; set; }
		int? Specialties { get; set; }
	}
}
