using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Changes
{
    public class ChangeItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("iso_639_1")]
        public string Iso_639_1 { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}