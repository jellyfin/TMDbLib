using Newtonsoft.Json;

namespace TMDbLib.Objects.General
{
    public class MediaKnownFor
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public MediaType Type { get; set; }
    }
}