using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    // TODO: Move into generic objects
    public class Translation
    {
        [JsonProperty("english_name")]
        public string EnglishName { get; set; }

        [JsonProperty("iso_639_1")]
        public string Iso_639_1 { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}