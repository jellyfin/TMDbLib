using Newtonsoft.Json;

namespace TMDbLib.Objects.Lists
{
    internal class ListStatus
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("item_present")]
        public bool ItemPresent { get; set; }
    }
}
