using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeyUrl.Models
{
	public class Click
	{
		[Required]
		public Guid Id { get; set; }
		[Required]
		public string ShortUrl { get; set; }
		[Required]
		public string Browser { get; set; }
		[Required]
		public string Platform { get; set; }

		[Required]
		[DataType(DataType.Date)]		
		public DateTime Clicked { get; set; }

  }
}
