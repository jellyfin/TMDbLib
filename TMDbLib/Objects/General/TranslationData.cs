using Newtonsoft.Json;

namespace TMDbLib.Objects.General
{
    public class TranslationData
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("tagline")]
        public string Tagline { get; set; }
    }
}
