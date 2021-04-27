using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageAbridged.Services.Core;
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
		private readonly IProcessInfo _processInfo;

		public ConvertImgController(IHostingEnvironment hostingEnvironment,
			IProcessInfo processInfo)
		{
			_hostingEnvironment = hostingEnvironment;
			_processInfo = processInfo;
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
					bitmapJpeg.MakeTransparent();
					using (MemoryStream ms = new MemoryStream())
					{
						bitmapJpeg.Save(ms, ImageFormat.Png);

						processedImages.Add(Convert.ToBase64String(ms.ToArray()));
					}
				}
			}

			return Ok(processedImages);
		}

		[HttpPost]
		[Route("ConvetImageToWebp")]
		public IActionResult ConvertImageToWebp(IFormFile[] imageFiles)
		{
			var processedImages = new List<string>();
			foreach (var imageFile in imageFiles)
			{
				var savedImagePath = _hostingEnvironment.ContentRootPath + "\\processedwebp\\" + imageFile.FileName;
				var extension = Path.GetExtension(savedImagePath);
				var savedWebpImagePath = new StringBuilder(_hostingEnvironment.ContentRootPath + "\\processedwebp\\");
				switch (extension.ToLower())
				{
					case ".png":
					case ".jpg":
						{
							savedWebpImagePath.Append(imageFile.FileName.Replace(extension.ToLower(), ".webp"));
							break;
						}
				};

				var savedOutputPath = savedWebpImagePath.ToString();

				using (var stream = new FileStream(savedImagePath, FileMode.Create))
				{
					imageFile.CopyTo(stream);
				}

				try
				{
					var exePath = _hostingEnvironment.ContentRootPath + "\\imgprocess\\webp\\" + "cwebp.exe";
					this._processInfo.ExecuteWindowRedirectProcess(exePath, string.Format("-q 80 -lossless \"{0}\" -o \"{1}\"", savedImagePath, savedOutputPath));
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}

				var processedPngFile = System.IO.File.ReadAllBytes(savedOutputPath);
				processedImages.Add(Convert.ToBase64String(processedPngFile));
			}

			return Ok(processedImages);
		}
	}
}