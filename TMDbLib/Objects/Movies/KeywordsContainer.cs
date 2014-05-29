using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    public class KeywordsContainer
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("keywords")]
        public List<Keyword> Keywords { get; set; }
    }
}