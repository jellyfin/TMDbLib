using Newtonsoft.Json;

namespace TMDbLib.Objects.General
{
    public class TranslationData
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("homepage")]
        public string HomePage { get; set; }

        [JsonProperty("tagline")]
        public string Tagline { get; set; }

        [JsonProperty("runtime")]
        public int Runtime { get; set; }
    }
}
