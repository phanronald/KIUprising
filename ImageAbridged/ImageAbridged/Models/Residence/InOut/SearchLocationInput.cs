using ImageAbridged.Models.Residence.Shared;

namespace ImageAbridged.Models.Residence.InOut
{
	public class SearchLocationInput : LatLng
	{
		public string SearchTerm { get; set; }
	}
}
