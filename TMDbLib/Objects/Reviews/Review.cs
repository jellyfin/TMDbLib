using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Reviews
{
    public class Review
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// A language code, e.g. en
        /// </summary>
        [JsonProperty("iso_639_1")]
        public string Iso_639_1 { get; set; }

        [JsonProperty("media_id")]
        public int MediaId { get; set; }

        [JsonProperty("media_title")]
        public string MediaTitle { get; set; }

        [JsonProperty("media_type")]
        public MediaType MediaType { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
