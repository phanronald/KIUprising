using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ImageAbridged.Models.Residence.Search.Listings;
using ImageAbridged.Models.Residence.Search.Region;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ImageAbridged.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ResidenceController : ControllerBase
	{
		public ResidenceController()
		{

		}

		[HttpPost]
		[Route("SearchGeoLocation")]
		public async Task<IActionResult> SearchGeoLocation(string searchTerm, double latitude, double longitude)
		{
			var allGeoLocationsFound = await GetSearchLocation(searchTerm, latitude, longitude);
			return Ok(allGeoLocationsFound);
		}

		[HttpPost]
		[Route("SearchApartments")]
		public async Task<IActionResult> SearchApartments(string searchTerm, double latitude, double longitude)
		{
			var geoLocationSearch = new List<GeoLocationSearch>();
			var apartmentSearchUrl = "https://www.apartments.com/services/url/";

			var searchGeoLocation = await GetSearchLocation(searchTerm, latitude, longitude);
			var listingFilter = new ListingFilters()
			{
				MinRentAmount = 500,
				MaxRentAmount = 750,
				MinBeds = 1,
				MaxBeds = 1,
				MinBaths = 1,
				PetFriendly = null,
				Style = null
			};

			using (var client = new HttpClient())
			{
				var content = new
				{
					Geography = searchGeoLocation.First(),
					Listing = listingFilter
				};

				var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
				using (var response = await client.PostAsync(apartmentSearchUrl, data))
				{
					var apiResponse = await response.Content.ReadAsStringAsync();
					var a = "";
					//geoLocationSearch = JsonConvert.DeserializeObject<List<GeoLocationSearch>>(apiResponse);
				}
			}

			return Ok();
		}

		private async Task<List<GeoLocationSearch>> GetSearchLocation(string searchTerm, double latitude, double longitude)
		{
			var geoLocationSearch = new List<GeoLocationSearch>();
			var apartmentGeoLocationSearchUrl = "https://www.apartments.com/services/geography/search";
			using (var client = new HttpClient())
			{
				//using (var aptResponse = await client.GetAsync(apartmentSearchUrl))
				//{
				//	using (var content = aptResponse.Content)
				//	{
				//		var data = await content.ReadAsStringAsync();
				//	}
				//}

				var content = new
				{
					t = searchTerm,
					l = new[] { longitude, latitude }
				};

				var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
				using (var response = await client.PostAsync(apartmentGeoLocationSearchUrl, data))
				{
					var apiResponse = await response.Content.ReadAsStringAsync();
					geoLocationSearch = JsonConvert.DeserializeObject<List<GeoLocationSearch>>(apiResponse);
				}
			}

			return geoLocationSearch;
		}
	}
}
