using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeyUrl.ViewModels
{
	public class CreateViewModel
	{  
        public Guid Id { get; set; }
        [Required]
        [RegularExpression(@"^[A-F]{5}$", ErrorMessage = "Incorrect short Url format in 5 letters")]
        public string ShortUrl { get; set; }
        [Required]
        public string OriginalUrl { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public string CreatedView 
        {
            get { return Created.ToString("MMM dd, yyyy"); }
        }
        public int Count { get; set; }
    }
}
