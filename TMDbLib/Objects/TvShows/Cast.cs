using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows
{
    public class Cast
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("character")]
        public string Character { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("credit_id")]
        public string CreditId { get; set; }
        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }
    }
}