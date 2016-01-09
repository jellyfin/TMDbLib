using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General
{
    public class PersonResult
    {
        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("known_for")]
        public List<MediaKnownFor> KnownFor { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("bson_id")]
        public string Bson_Id { get; set; }

        [JsonProperty("profile-path")]
        public string ProfilePath { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}