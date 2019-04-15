using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
		public void CompressPng(IFormFile[] pngImgFiles)
		{
			var startInfo = new ProcessStartInfo();
			foreach (var pngImgFile in pngImgFiles)
			{
				var savedImagePath = _hostingEnvironment.ContentRootPath + "\\processedpngs\\" + pngImgFile.FileName;
				using (var stream = new FileStream(savedImagePath, FileMode.Create))
				{
					pngImgFile.CopyTo(stream);
				}

				try
				{
					var exePath = _hostingEnvironment.ContentRootPath + "\\imgprocess\\" + "pngquant.exe";
					ExecuteProcess(exePath, string.Format("-f -v --ext .png --quality 60-80 \"{0}\"", savedImagePath));
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}
			}

			//var image = System.IO.File.OpenRead(savedImagePath);
			//return File(image, "image/png");
		}

		private static void ExecuteProcess(string filename, string arguments)
		{
			using (var process = new Process
			{
				StartInfo = { FileName = filename, Arguments = arguments, CreateNoWindow = false, ErrorDialog = false, UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true, WindowStyle = ProcessWindowStyle.Hidden },
				EnableRaisingEvents = true
			})
			{
				process.Start();
				process.WaitForExit();
			}
		}
	}
}