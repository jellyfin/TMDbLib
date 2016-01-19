using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.People
{
    public class MovieCredits
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("cast")]
        public List<MovieRole> Cast { get; set; }

        [JsonProperty("crew")]
        public List<MovieJob> Crew { get; set; }
    }
}