using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageAbridged.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConvertImgController : ControllerBase
	{
		private readonly IHostingEnvironment _hostingEnvironment;

		public ConvertImgController(IHostingEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
		}

		[HttpPost]
		[Route("ConvetJpgToPng")]
		public IActionResult CompressJpeg(IFormFile[] jpgImgFiles)
		{
			var processedImages = new List<string>();

			foreach (var jpgImgFile in jpgImgFiles)
			{
				using (var stream = jpgImgFile.OpenReadStream())
				{
					var bitmapJpeg = (Bitmap)Bitmap.FromStream(stream);
					using (MemoryStream ms = new MemoryStream())
					{
						bitmapJpeg.Save(ms, ImageFormat.Png);

						processedImages.Add(Convert.ToBase64String(ms.ToArray()));
					}
				}
			}

			return Ok(processedImages);
		}
	}
}