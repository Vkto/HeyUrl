using HeyUrl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HeyUrl.Utils;
using HeyUrl.ViewModels;
using HeyUrl.Data;
using Microsoft.EntityFrameworkCore;
using Shyjus.BrowserDetection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace HeyUrl.Controllers
{
	public class UrlsController : Controller
	{
		private readonly ILogger<UrlsController> _logger;
		private readonly HeyUrlContext _context;
		private readonly IBrowserDetector _browserDetector;
				
		public UrlsController(ILogger<UrlsController> logger, HeyUrlContext context, IBrowserDetector browserDetector)
		{
			_logger = logger;
			_context = context;
			_browserDetector = browserDetector;
		}

		public async Task<IActionResult> Index()
		{
			string host = HttpContext.Request.Host.Value;
			string schema = HttpContext.Request.Scheme;

			string shema_short_url = $"{schema}://{host}";

			TempData["Host"] = shema_short_url;

			List<CreateViewModel> cvmList = new();

      List<Url> urlList = await _context.Url.ToListAsync();

			foreach (Url url in urlList)
			{
				CreateViewModel cvm = new();

				cvm.Id = url.Id;
				cvm.ShortUrl = url.ShortUrl;
				cvm.OriginalUrl = url.OriginalUrl;
				cvm.Created = url.Created;
				cvm.Count = _context.Click.Count(c => c.ShortUrl == url.ShortUrl);

				cvmList.Add(cvm);	
			}

      return View(cvmList.OrderByDescending(u => u.Created));
		}


		[Route("Urls/Create")]
		public IActionResult Create(string originalUrl)
		{
			if (originalUrl != null)
			{
				originalUrl = originalUrl.Trim();
			}

			TempData["OriginalUrl"] = originalUrl;
			TempData["Notice"] = null;

			var valRes = UrlUtils.IsValidUrl(originalUrl);

			if (valRes.Item1)
			{
				string shortUrl = UrlUtils.GetShortUrl(originalUrl);

				var url = _context.Url.FirstOrDefault(u => u.ShortUrl == shortUrl);

				if (url == null)
				{
					var model = new CreateViewModel();
					model.Id = Guid.NewGuid();
					model.ShortUrl = shortUrl;
					model.OriginalUrl = originalUrl;
					model.Created = DateTime.Today;
					
					return View(model);
				}
				else
				{
					TempData["Notice"] = $"This url has already been shorten as {url.ShortUrl}";
				}
			}
			else
			{
				TempData["Notice"] = valRes.Item2;
			}
			
			return RedirectToAction(nameof(Index));
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("Urls/Save")]
		public async Task<IActionResult> Save(CreateViewModel cvm)
		{
			if (ModelState.IsValid)
			{
				Url url = new Url
				{
					Id = Guid.NewGuid(),
					ShortUrl = cvm.ShortUrl,
					OriginalUrl = cvm.OriginalUrl,
					Created = DateTime.Now,
					Clicks = 0
				};

				_context.Add(url);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return RedirectToAction(nameof(Error));
		}

		[Route("/{url}")]
		public IActionResult Visit(string url)
		{
			TempData["ShortUrl"] = url;

			return RedirectToAction("Visit", "Clicks");
		}

		// GET: Urls/Delete/ShortUrl
		public async Task<IActionResult> Delete(string Id)
		{			
			string shortUrl = Id;

			if (shortUrl == null || !UrlUtils.IsValidShortUrl(shortUrl))
			{
				return NotFound();
			}

			//delete from Url table
			Url url = await _context.Url
				.FirstOrDefaultAsync(m => m.ShortUrl == shortUrl);

			if (url == null)
			{
				return NotFound();
			}

			_context.Url.Remove(url);

			//delete from Click table
			var clickList = _context.Click.Where(m => m.ShortUrl == shortUrl).ToList();
			
			if (url != null && clickList.Count > 0)
			{
				_context.Click.RemoveRange(clickList);
			}

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
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
