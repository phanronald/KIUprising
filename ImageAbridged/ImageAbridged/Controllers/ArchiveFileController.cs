using System;
using System.Collections.Generic;
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
	public class ArchiveFileController : ControllerBase
	{
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IProcessInfo _processInfo;

		public ArchiveFileController(IHostingEnvironment hostingEnvironment,
									IProcessInfo processInfo)
		{
			_hostingEnvironment = hostingEnvironment;
			_processInfo = processInfo;
		}

		[HttpPost]
		[Route("ArchiveSevenZip")]
		public IActionResult ArchiveSevenZip(IFormFile[] zipFiles)
		{
			var savedInitArchivePath = _hostingEnvironment.ContentRootPath + "\\processedarchives\\";
			var savedArchivedPath = string.Empty;
			var newlySavedArchivePath = string.Empty;
			foreach (var zipFile in zipFiles)
			{
				savedArchivedPath = savedInitArchivePath + zipFile.FileName;
				newlySavedArchivePath = savedInitArchivePath + Guid.NewGuid();

				using (var stream = new FileStream(savedArchivedPath, FileMode.Create))
				{
					zipFile.CopyTo(stream);
				}

				try
				{
					var exePath = _hostingEnvironment.ContentRootPath + "\\archiveprocess\\" + "7z.exe";
					this._processInfo.ExecuteNoWindowRedirectProcess(exePath, string.Format("x \"{0}\" -y -o\"{1}\"", savedArchivedPath, newlySavedArchivePath));
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}

			}

			var filesInsideZip = this.DirectorySearch(newlySavedArchivePath)
				.GroupBy(g => g.Key.Substring(0, newlySavedArchivePath.Length))
				.ToDictionary(k => k.Key, v => v.ToDictionary(k1 => k1.Key, v1 => v1.Value));

			return Ok(filesInsideZip);
		}

		private Dictionary<string, string[]> DirectorySearch(string directoryPath)
		{
			var directoryFileLookup = new Dictionary<string, string[]>();
			try
			{
				foreach (var subDirectory in Directory.GetDirectories(directoryPath))
				{
					var fileInDirectory = new List<string>();
					foreach (var file in Directory.GetFiles(subDirectory))
					{
						fileInDirectory.Add(file);
					}

					directoryFileLookup.Add(subDirectory, fileInDirectory.ToArray());
					var moreSubDirectories = this.DirectorySearch(subDirectory);
					if (moreSubDirectories.Any())
					{
						foreach (var moreSubDirectory in moreSubDirectories)
						{
							directoryFileLookup.Add(moreSubDirectory.Key, moreSubDirectory.Value);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return directoryFileLookup;
		}
	}
}