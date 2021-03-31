using System;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WebFileManager.Models;

namespace WebFileManager.Controllers
{
	public class ContentController : Controller
	{
		private readonly AppSettings settings;

		public ContentController(IOptions<AppSettings> options)
		{
			this.settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[RequestSizeLimit(1_073_741_824)]
		public async Task<IActionResult> Index([FromForm] UploadFileViewModel model)
		{
			// CF TEMP
			foreach (var modelState in ViewData.ModelState.Values)
			{
				foreach (var error in modelState.Errors)
				{
				}
			}

			if (!ModelState.IsValid)
			{
				return View();
			}

			var file = model.File;

			var filePath = Path.Combine(settings.RootDirectory, file.FileName);

			// TODO: Fix vulnerability.
#pragma warning disable CA3003 // Review code for file path injection vulnerabilities
			await using var fileStream = System.IO.File.Create(filePath);
#pragma warning restore CA3003 // Review code for file path injection vulnerabilities

			await model.File.CopyToAsync(fileStream);

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
