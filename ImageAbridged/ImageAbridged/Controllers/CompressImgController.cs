using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ImageAbridged.Services.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageAbridged.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompressImgController : ControllerBase
	{
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IProcessInfo _processInfo;

		public CompressImgController(IHostingEnvironment hostingEnvironment,
									IProcessInfo processInfo)
		{
			_hostingEnvironment = hostingEnvironment;
			_processInfo = processInfo;
		}

		[HttpPost]
		[Route("CompressPng")]
		public IActionResult CompressPng(IFormFile[] pngImgFiles)
		{
			var processedImages = new List<string>();
			foreach (var pngImgFile in pngImgFiles)
			{
				var savedImagePath = _hostingEnvironment.ContentRootPath + "\\processedpngs\\" + pngImgFile.FileName;
				using (var stream = new FileStream(savedImagePath, FileMode.Create))
				{
					pngImgFile.CopyTo(stream);
				}

				try
				{
					var exePath = _hostingEnvironment.ContentRootPath + "\\imgprocess\\png\\" + "pngquant.exe";
					this._processInfo.ExecuteWindowRedirectProcess(exePath, string.Format("-f -v --ext .png --quality 60-80 \"{0}\"", savedImagePath));
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}

				var processedPngFile = System.IO.File.ReadAllBytes(savedImagePath);
				processedImages.Add(Convert.ToBase64String(processedPngFile));
			}

			return Ok(processedImages);
		}

		[HttpPost]
		[Route("CompressJpeg")]
		public IActionResult CompressJpeg(IFormFile[] jpgImgFiles)
		{
			var processedImages = new List<string>();
			var savedImagePath = _hostingEnvironment.ContentRootPath + "\\processedjpgs\\";
			foreach (var jpgImgFile in jpgImgFiles)
			{
				var savedImage = savedImagePath + jpgImgFile.FileName;
				var savedCompressedImagePath = new StringBuilder(savedImagePath + Guid.NewGuid());
				switch (jpgImgFile.ContentType)
				{
					case "image/jpg":
						{
							savedCompressedImagePath.Append(".jpg");
							break;
						}
					case "image/jpeg":
						{
							savedCompressedImagePath.Append(".jpeg");
							break;
						}
				}
				using (var stream = new FileStream(savedImage, FileMode.Create))
				{
					jpgImgFile.CopyTo(stream);
				}

				try
				{
					var exePath = _hostingEnvironment.ContentRootPath + "\\imgprocess\\jpg\\" + "cjpeg.exe";
					this._processInfo.ExecuteNoWindowRedirectProcess(exePath, string.Format("-quality 80 -outfile \"" + savedCompressedImagePath.ToString() + "\" " + "\"" + savedImage + "\""));
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}

				var processedPngFile = System.IO.File.ReadAllBytes(savedCompressedImagePath.ToString());
				processedImages.Add(Convert.ToBase64String(processedPngFile));
			}

			return Ok(processedImages);
		}
	}
}
