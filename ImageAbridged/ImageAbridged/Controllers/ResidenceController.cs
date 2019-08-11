using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ImageAbridged.Models.Residence.InOut;
using ImageAbridged.Models.Residence.Search.Listings;
using ImageAbridged.Models.Residence.Search.Region;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenScraping;
using OpenScraping.Config;

namespace ImageAbridged.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ResidenceController : ControllerBase
	{
		private readonly IHostingEnvironment _hostingEnvironment;

		public ResidenceController(IHostingEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
		}

		[HttpPost]
		[Route("SearchGeoLocation")]
		public async Task<IActionResult> SearchGeoLocation(SearchLocationInput searchLocationInput)
		{
			var allGeoLocationsFound = await GetSearchLocation(searchLocationInput.SearchTerm,
				searchLocationInput.Latitude, searchLocationInput.Longitude);

			return Ok(allGeoLocationsFound);
		}

		[HttpPost]
		[Route("SearchApartmentUrl")]
		public async Task<IActionResult> SearchApartmentUrl(SearchApartmentInput searchApartmentInput)
		{
			var searchGeoLocation = await GetSearchLocation(searchApartmentInput.SearchTerm,
				searchApartmentInput.Latitude, searchApartmentInput.Longitude);

			var listingFilter = new ListingFilters()
			{
				MinRentAmount = searchApartmentInput.MinRentAmount,
				MaxRentAmount = searchApartmentInput.MaxRentAmount,
				MinBeds = searchApartmentInput.MinBeds,
				MaxBeds = searchApartmentInput.MaxBeds,
				MinBaths = searchApartmentInput.MinBaths,
				PetFriendly = searchApartmentInput.PetFriendly,
				Style = searchApartmentInput.Style
			};

			var searchAptOutput = await GetSearchApiUrl(searchGeoLocation, listingFilter);

			return Ok(searchAptOutput);
		}

		[HttpPost]
		[Route("SearchApartments")]
		public async Task<IActionResult> SearchApartments(SearchApartmentInput searchApartmentInput)
		{
			var searchGeoLocation = await GetSearchLocation(searchApartmentInput.SearchTerm,
				searchApartmentInput.Latitude, searchApartmentInput.Longitude);

			var listingFilter = new ListingFilters()
			{
				MinRentAmount = searchApartmentInput.MinRentAmount,
				MaxRentAmount = searchApartmentInput.MaxRentAmount,
				MinBeds = searchApartmentInput.MinBeds,
				MaxBeds = searchApartmentInput.MaxBeds,
				MinBaths = searchApartmentInput.MinBaths,
				PetFriendly = searchApartmentInput.PetFriendly,
				Style = searchApartmentInput.Style
			};

			var searchAptOutput = await GetSearchApiUrl(searchGeoLocation, listingFilter);

			var htmlResult = string.Empty;
			using (var client = new HttpClient())
			{
				using (var response = client.GetAsync(searchAptOutput.Url).Result)
				{
					using (HttpContent content = response.Content)
					{
						htmlResult = content.ReadAsStringAsync().Result;
					}
				}
			}

			var aptSearchConfigJson = _hostingEnvironment.ContentRootPath + "\\Configs\\Residence\\searchapartmentconfig.json";
			using (var sr = new StreamReader(aptSearchConfigJson))
			{
				var configJson = sr.ReadToEnd();
				var config = StructuredDataConfig.ParseJsonString(configJson);
				var openScraping = new StructuredDataExtractor(config);
				var scrapingResults = openScraping.Extract(htmlResult);
				var a = scrapingResults["apartments"];
			}

			return Ok(searchAptOutput);
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

		private async Task<SearchAptOutput> GetSearchApiUrl(List<GeoLocationSearch> searchGeoLocation, IListingFilters listingFilters)
		{
			var apartmentSearchUrl = "https://www.apartments.com/services/url/";
			var searchAptOutput = new SearchAptOutput();
			using (var client = new HttpClient())
			{
				var content = new
				{
					Geography = searchGeoLocation.First(),
					Listing = listingFilters
				};

				var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
				using (var response = await client.PostAsync(apartmentSearchUrl, data))
				{
					var apiResponse = await response.Content.ReadAsStringAsync();
					searchAptOutput = JsonConvert.DeserializeObject<SearchAptOutput>(apiResponse);
				}
			}

			return searchAptOutput;
		}
	}
}
