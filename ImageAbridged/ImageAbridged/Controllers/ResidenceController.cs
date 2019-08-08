using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ImageAbridged.Models.Residence.Search;
using Microsoft.AspNetCore.Http;
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
		[Route("SearchApartments")]
		public async Task<IActionResult> SearchApartments()
		{
			var geoLocationSearch = new List<GeoLocationSearch>();
			var apartmentSearchUrl = "https://www.apartments.com/services/geography/search";
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
					t = "lax",
					l = new[] { -95.432, 29.817 }
				};

				var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
				using (var response = await client.PostAsync(apartmentSearchUrl, data))
				{
					var apiResponse = await response.Content.ReadAsStringAsync();
					geoLocationSearch = JsonConvert.DeserializeObject<List<GeoLocationSearch>>(apiResponse);
				}
			}

			return Ok(geoLocationSearch);
		}
	}
}