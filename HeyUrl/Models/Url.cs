using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeyUrl.Models
{
	public class Url
	{        
        public Guid Id { get; set; }
        [Required]
        [RegularExpression(@"^[A-F]{5}$")]
        public string ShortUrl { get; set; }
        [Required]
        [RegularExpression(@"^((http://)|(https://))*[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$")]  
        public string OriginalUrl { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
        [Required]
        public int Clicks { get; set; }


    }
}
