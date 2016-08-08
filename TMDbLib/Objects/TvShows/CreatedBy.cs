using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows
{
    public class CreatedBy
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }
    }
}