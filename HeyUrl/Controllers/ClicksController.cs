using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HeyUrl.Data;
using HeyUrl.Models;
using Shyjus.BrowserDetection;
using HeyUrl.ViewModels;
using HeyUrl.Utils;

namespace HeyUrl.Controllers
{
  public class ClicksController : Controller
  {
    private readonly HeyUrlContext _context;
    private readonly IBrowserDetector _browserDetector;

    public ClicksController(HeyUrlContext context, IBrowserDetector browserDetector)
    {
      _context = context;
      _browserDetector = browserDetector;
    }

    [Route("Clicks/Show/{url}/{count}")]
    public IActionResult Show(string url, int count)
    {
      return ShowMessages(url, count);
    }

    private IActionResult ShowMessages(string url, int count)
    {
      if (string.IsNullOrWhiteSpace(url) || url.Length != 5)
      {
        TempData["Notice"] = $"Wrong Url: {url}";
      }
      else if (count == 0)
      {
        TempData["Notice"] = $"The URL {url} has no statistics because it was not accessed";
      }
      else
      {
        return ReturnViewModel(url);
      }

      return RedirectToAction("Index", "Urls");
    }

    [Route("/Clicks/{shortUrl}")]
    public async Task<IActionResult> Visit(string shortUrl)
    {
      if (shortUrl == "Visit" && TempData["ShortUrl"] != null)
      {
        shortUrl = TempData["ShortUrl"].ToString();
      }

      bool isValid = UrlUtils.IsValidShortUrl(shortUrl);

      if (isValid)
      {
        return await ReturnRedirect(shortUrl);
      }
      else
      {
        return NotFound();
      }
    }

    private async Task<IActionResult> ReturnRedirect(string shortUrl)
    {
      return await ReturnRedirects(shortUrl);
    }

    private async Task<IActionResult> ReturnRedirects(string shortUrl)
    {
      Url url = _context.Url.FirstOrDefault(url => url.ShortUrl == shortUrl);

      if (url != null)
      {
        Click click = new Click
        {
          Id = Guid.NewGuid(),
          ShortUrl = shortUrl,
          Browser = _browserDetector.Browser.Name,
          Platform = _browserDetector.Browser.OS,
          Clicked = DateTime.Today
        };

        _context.Add(click);
        await _context.SaveChangesAsync();

        UriBuilder uriBuilder = new UriBuilder(url.OriginalUrl.Trim());
        return Redirect(uriBuilder.Uri.ToString());
      }
      else
      {
        return NotFound();
      }
    }

    private IActionResult ReturnViewModel(string url)
    {
      int totalCount = _context.Click.Count(c => c.ShortUrl == url);

      Url urlStatus = _context.Url.FirstOrDefault(u => u.ShortUrl == url);

      string originalUrl = _context.Url.FirstOrDefault(u => u.ShortUrl == url).OriginalUrl;

      Url viewUrl = new Url { ShortUrl = url, OriginalUrl = urlStatus.OriginalUrl, Created = urlStatus.Created, Clicks = totalCount };

      int year = DateTime.Today.Year;
      int month = DateTime.Today.Month;
      DateTime firstMonthDay = new DateTime(year, month, 1);

      List<Click> clickList = _context.Click.Where(c => c.ShortUrl == url && c.Clicked >= firstMonthDay).ToList();

      Dictionary<string, int> dailyClicks = ReturnDailyClicks(clickList);

      Dictionary<string, int> browserClicks = ReturnBrowserClicks(clickList);

      Dictionary<string, int> platformClicks = ReturnPlatformsClicks(clickList);

      return View(new ShowViewModel
      {
        Url = viewUrl,
        DailyClicks = dailyClicks,
        BrowserClicks = browserClicks,
        PlatformClicks = platformClicks,
      });
    }

    private static Dictionary<string, int> ReturnDailyClicks(List<Click> clickList)
    {

      //DailyClicks
      Dictionary<string, int> dailyClicks = new Dictionary<string, int>();

      foreach (IGrouping<DateTime, Click> click in clickList.GroupBy(click => click.Clicked))
      {
        dailyClicks.Add(click.Key.ToString(), click.Count());
      }

      return dailyClicks;
    }

    private static Dictionary<string, int> ReturnBrowserClicks(List<Click> clickList)
    {
      Dictionary<string, int> browserClicks = new Dictionary<string, int>();
      foreach (var br in from b in clickList
                         group b by b.Browser into bGroup
                         select new
                         {
                           Browser = bGroup.Key,
                           Count = bGroup.Count()
                         })
      {
        browserClicks.Add(br.Browser, br.Count);
      }

      return browserClicks;
    }

    private static Dictionary<string, int> ReturnPlatformsClicks(List<Click> clickList)
    {
      Dictionary<string, int> platformClicks = new Dictionary<string, int>();


      foreach (var pl in from p in clickList
                         group p by p.Platform into pGroup
                         select new Platform
                         {
                           OS = pGroup.Key,
                           Count = pGroup.Count()
                         })
      {
        platformClicks.Add(pl.OS, pl.Count);
      }

      return platformClicks;
    }
  }
}
