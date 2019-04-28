using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		public ArchiveFileController(IHostingEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
		}

		[HttpPost]
		[Route("ArchiveSevenZip")]
		public IActionResult ArchiveSevenZip()
		{
			return Ok();
		}
	}
}