﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Podcasts
{
    public class PodcastUpdateRequest : PodcastAddRequest, IModelIdentifier
    {
        public int Id { get; set; }
    }
}
