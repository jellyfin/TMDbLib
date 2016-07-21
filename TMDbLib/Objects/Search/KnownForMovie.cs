using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Search
{
    public class KnownForMovie : KnownForBase
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("video")]
        public bool Vide { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }
    }
}