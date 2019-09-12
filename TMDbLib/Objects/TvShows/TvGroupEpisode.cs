using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows
{
    public class TvGroupEpisode : TvEpisodeBase
    {
        [JsonProperty("order")]
        public int Order { get; set; }
    }
}