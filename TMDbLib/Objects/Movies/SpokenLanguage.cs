using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    public class SpokenLanguage
    {
        [JsonProperty("iso_639_1")]
        public string Iso_639_1 { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}