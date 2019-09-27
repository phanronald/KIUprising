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
using ImageAbridged.Models.Residence.Search.Details;

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

		[HttpPost]
		[Route("SearchApartmentDetail")]
		public async Task<IActionResult> SearchApartmentDetail(string apartmentUrl)
		{
			var htmlResult = string.Empty;
			using (var client = new HttpClient())
			{
				using (var response = client.GetAsync(apartmentUrl).Result)
				{
					using (HttpContent content = response.Content)
					{
						htmlResult = content.ReadAsStringAsync().Result;
					}
				}
			}

			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(htmlResult);
			GetDetailHtmlExtracted(htmlDocument.DocumentNode);

			return Ok(string.Empty);
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
				var contentContainerArticle = placardListing.CssSelect("article");
				if (contentContainerArticle.Any())
				{
					var section = contentContainerArticle.First();

					#region Media

					var mediaInContent = section.CssSelect("div.imageContainer div.carouselInner div.item ");
					if (mediaInContent.Any())
					{
						var allImageUrls = new List<string>();
						foreach (var mediaImage in mediaInContent)
						{
							var url = Regex.Match(mediaImage.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))").Groups[1].Value.Trim('"');
							allImageUrls.Add(url);
						}

						placard.ImageUrls = allImageUrls;
					}

					#endregion

					#region Property Info
					var titleInContent = section.CssSelect("a.placardTitle");
					if (titleInContent.Any())
					{
						var titleContainer = titleInContent.First();
						placard.Title = titleContainer.InnerText.Trim('\r', '\n');
						placard.DetailPageUrl = titleContainer.GetAttributeValue("href", "").Trim('"');
					}

					var addressInContent = section.CssSelect("div.location");
					if (addressInContent.Any())
					{
						placard.Location = addressInContent.First().InnerText.Trim('\r', '\n');
					}

					var rentPriceInContent = section.CssSelect("span.altRentDisplay");
					if (rentPriceInContent.Any())
					{
						placard.RentRange = rentPriceInContent.First().InnerText;
					}

					var unitInContent = section.CssSelect("span.unitLabel ");
					if (unitInContent.Any())
					{
						placard.ResidentUnit = unitInContent.First().InnerText;
					}

					var availabilityInContent = section.CssSelect("span.availabilityDisplay");
					if (availabilityInContent.Any())
					{
						placard.Availability = availabilityInContent.First().InnerText;
					}

					#endregion
				}

				allPlacards.Add(placard);
			}
		}

		private void GetDetailHtmlExtracted(HtmlNode htmlDocumentNode)
		{
			var residenceDetail = new Residence();
			var headerInfoSectionContent = htmlDocumentNode.CssSelect("header.propertyHeader.screen");
			if (headerInfoSectionContent.Any())
			{
				var headerInfoSection = headerInfoSectionContent.First();
				var residenceNameContainer = headerInfoSection.CssSelect(".propertyName");
				if (residenceNameContainer.Any())
				{
					residenceDetail.Name = residenceNameContainer.First().InnerText.Trim('\r', '\n');
				}

				var residenceAddressContainer = headerInfoSection.CssSelect(".propertyAddress");
				if (residenceAddressContainer.Any())
				{
					var residenceAddressNameContainer = residenceAddressContainer.CssSelect("h2");
					if (residenceAddressNameContainer.Any())
					{
						var sb = new StringBuilder();
						var location = residenceAddressNameContainer.First().InnerText.Trim('\r', '\n').Trim();
						var locationsPure = location.Split(",", System.StringSplitOptions.RemoveEmptyEntries);

						var index = 0;
						foreach (var locationPure in locationsPure)
						{
							if (index != 0)
							{
								sb.Append(", ");
							}

							sb.Append(Regex.Replace(locationPure, "\r\n\\s+", " "));
							index++;
						}

						residenceDetail.Location = sb.ToString();
					}
				}
			}

			var rentRollupSectionContainer = htmlDocumentNode.CssSelect("div.rentRollupContainer");
			if (rentRollupSectionContainer.Any())
			{
				var rentListinInfo = new List<string>();
				var rentDetailInfoContainer = rentRollupSectionContainer.First();
				var allRentInfo = rentDetailInfoContainer.CssSelect("span.rentRollup");
				foreach(var rentInfo in allRentInfo)
				{
					rentListinInfo.Add(Regex.Replace(rentInfo.InnerHtml, @"<[^>]*>", string.Empty));
				}
			}
		}
	}
}
