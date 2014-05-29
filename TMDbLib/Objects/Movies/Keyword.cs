using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    public class Keyword
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}