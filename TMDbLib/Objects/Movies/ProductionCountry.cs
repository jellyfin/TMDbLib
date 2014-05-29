using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    public class ProductionCountry
    {
        [JsonProperty("iso_3166_1")]
        public string Iso_3166_1 { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}