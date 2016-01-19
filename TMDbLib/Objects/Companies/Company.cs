using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Companies
{
    public class Company
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("headquarters")]
        public string Headquarters { get; set; }

        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("logo_path")]
        public string LogoPath { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_company")]
        public ParentCompany ParentCompany { get; set; }

        [JsonProperty("movies")]
        public SearchContainer<MovieResult> Movies { get; set; }
    }
}