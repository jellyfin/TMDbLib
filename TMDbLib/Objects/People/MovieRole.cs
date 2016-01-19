using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.People
{
    public class MovieRole
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("character")]
        public string Character { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("credit_id")]
        public string CreditId { get; set; }
    }
}