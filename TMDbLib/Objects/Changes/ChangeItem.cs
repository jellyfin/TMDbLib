using System;
using Newtonsoft.Json;
using TMDbLib.Helpers;

namespace TMDbLib.Objects.Changes
{
    public class ChangeItem
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("iso_639_1")]
        public string Iso_639_1 { get; set; }

        [JsonProperty("time")]
        [JsonConverter(typeof(TmdbUtcTimeConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }
}