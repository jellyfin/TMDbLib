using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    public class ProductionCompany
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
