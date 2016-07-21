using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Search
{
    public class KnownForTv : KnownForBase
    {
        [JsonProperty("first_air_date")]
        public DateTime? FirstAirDate { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("original_name")]
        public string OriginalName { get; set; }

        [JsonProperty("origin_country")]
        public List<string> OriginCountry { get; set; }
    }
}