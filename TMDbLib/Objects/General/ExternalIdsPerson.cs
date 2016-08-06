using Newtonsoft.Json;

namespace TMDbLib.Objects.General
{
    public class ExternalIdsPerson : ExternalIds
    {
        [JsonProperty("imdb_id")]
        public string ImdbId { get; set; }

        [JsonProperty("facebook_id")]
        public string FacebookId { get; set; }

        [JsonProperty("twitter_id")]
        public string TwitterId { get; set; }
    }
}