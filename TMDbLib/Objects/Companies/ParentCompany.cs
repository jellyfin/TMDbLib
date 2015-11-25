using Newtonsoft.Json;

namespace TMDbLib.Objects.Companies
{
    public class ParentCompany
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("logo_path")]
        public string LogoPath { get; set; }
    }
}