using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageAbridged.Models;
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

		public CompressImgController(IHostingEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
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
					ExecuteWindowRedirectProcess(exePath, string.Format("-f -v --ext .png --quality 60-80 \"{0}\"", savedImagePath));
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
					ExecuteNoWindowRedirectProcess(exePath, string.Format("-quality 80 -outfile \"" + savedCompressedImagePath.ToString() + "\" " + "\"" + savedImage + "\""));
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

		private void ExecuteNodeProcess()
		{
			using (var process = new Process
			{
				StartInfo = { CreateNoWindow = true, RedirectStandardInput = true,
							  RedirectStandardOutput = true, UseShellExecute = false,
							  RedirectStandardError = true, FileName = "node.exe",
							  Arguments = "-i"}
			})
			{
				process.Start();
				process.WaitForExit();
			}
		}

		private void ExecuteWindowRedirectProcess(string filename, string arguments)
		{
			using (var process = new Process
			{
				StartInfo = { FileName = filename, Arguments = arguments,
							  CreateNoWindow = false, ErrorDialog = false,
							  UseShellExecute = false, RedirectStandardOutput = true,
							  RedirectStandardError = true,
							  WindowStyle = ProcessWindowStyle.Hidden },
				EnableRaisingEvents = true
			})
			{
				process.Start();
				process.WaitForExit();
			}
		}

		private void ExecuteNoWindowRedirectProcess(string filename, string arguments)
		{
			using (var process = new Process
			{
				StartInfo = { FileName = filename, Arguments = arguments,
							  CreateNoWindow = true, ErrorDialog = false,
							  UseShellExecute = false, RedirectStandardOutput = false,
							  RedirectStandardError = false },
				EnableRaisingEvents = true
			})
			{
				process.Start();
				process.WaitForExit();
			}
		}
	}
}
