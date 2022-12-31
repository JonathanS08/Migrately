using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Podcasts
{
    public class PodcastAddRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Description { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Link { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string CoverPhoto { get; set; }

    }
}
