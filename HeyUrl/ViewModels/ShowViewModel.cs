using HeyUrl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyUrl.ViewModels
{
	public class ShowViewModel
	{
		public Url Url { get; set; }
		public Dictionary<string, int> DailyClicks { get; set; }
		public Dictionary<string, int> BrowserClicks { get; set; }
		public Dictionary<string, int> PlatformClicks { get; set; }
	}
}
