using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Search
{
    public class SearchTv : MediaBase
    {
        [JsonProperty("first_air_date")]
        public DateTime? FirstAirDate { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("original_name")]
        public string OriginalName { get; set; }

        /// <summary>
        /// Country ISO code ex. US
        /// </summary>
        [JsonProperty("origin_country")]
        public List<string> OriginCountry { get; set; }
    }
}