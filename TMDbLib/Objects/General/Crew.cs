using Newtonsoft.Json;

namespace TMDbLib.Objects.General
{
    public class Crew
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("credit_id")]
        public string CreditId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("job")]
        public string Job { get; set; }
        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }
    }
}