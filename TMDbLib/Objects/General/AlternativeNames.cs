using Newtonsoft.Json;
using System.Collections.Generic;

namespace TMDbLib.Objects.General
{
    public class AlternativeNames
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("results")]
        public List<AlternativeName> Results { get; set; }
    }

    public class AlternativeName
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}