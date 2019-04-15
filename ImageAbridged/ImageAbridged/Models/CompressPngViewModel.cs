using Microsoft.AspNetCore.Http;

namespace ImageAbridged.Models
{
	public class CompressPngViewModel
	{
		public IFormFile pngImgFile { get; set; }
	}
}
