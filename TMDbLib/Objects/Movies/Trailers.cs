using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    public class Trailers
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("quicktime")]
        public List<object> Quicktime { get; set; } // TODO: Fix object type
        [JsonProperty("youtube")]
        public List<Youtube> Youtube { get; set; }
    }
}