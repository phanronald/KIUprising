using System.Collections.Generic;
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
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Text.RegularExpressions;

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

			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(htmlResult);
			GetHtmlExtracted(htmlDocument.DocumentNode);

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

		private void GetHtmlExtracted(HtmlNode htmlDocumentNode)
		{
			var allPlacards = new List<Placard>();

			var allPlacardContentListing = htmlDocumentNode.CssSelect("div#placardContainer ul li");
			foreach (var placardListing in allPlacardContentListing)
			{
				var placard = new Placard();
				var sectionInContent = placardListing.CssSelect("article section.placardContent");
				if(sectionInContent.Any())
				{
					var section = sectionInContent.First();

					#region Media

					var mediaInContent = section.CssSelect("div.imageContainer div.item.active ");
					if (mediaInContent.Any())
					{
						var mediaImage = mediaInContent.First();
						var url = Regex.Match(mediaImage.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))").Groups[1].Value.Trim('"');
						placard.ImageUrl = url;
					}

					#endregion

					#region Property Info
					var titleInContent = section.CssSelect("a.placardTitle");
					if(titleInContent.Any())
					{
						placard.Title = titleInContent.First().InnerText.Trim('\r', '\n');
					}

					var addressInContent = section.CssSelect("div.location");
					if(addressInContent.Any())
					{
						placard.Location = addressInContent.First().InnerText.Trim('\r', '\n');
					}

					#endregion
				}

				allPlacards.Add(placard);
			}
		}
	}
}
