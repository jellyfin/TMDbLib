using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Search
{
    public class SearchMulti
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { set { Name = value; } }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("original_name")]
        public string OriginalName { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { set { OriginalName = value; } }

        [JsonProperty("first_air_date")]
        public DateTime? FirstAirDate { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("type")]
        public MediaType Type { get; set; }
        
        [JsonProperty("origin_country")]
        public List<string> OriginCountry { get; set; }

        [JsonProperty("media_type")]
        public string MediaType
        {
            set
            {
                switch (value)
                {
                    case "tv":
                        Type = General.MediaType.Tv;
                        break;
                    case "movie":
                        Type = General.MediaType.Movie;
                        break;
                    default:
                        Type = General.MediaType.Unknown;
                        break;
                }
            }
        }
    }
}
