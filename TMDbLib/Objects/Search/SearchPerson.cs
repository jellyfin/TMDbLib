using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Search
{
    public class SearchPerson
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }


        [JsonProperty("known_for")]
        public List<SearchMovie> KnownFor { get; set; }
    }
}