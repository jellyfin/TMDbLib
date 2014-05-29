using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    public class Youtube
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("size")]
        public string Size { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}