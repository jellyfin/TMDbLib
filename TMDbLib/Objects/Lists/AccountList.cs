using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Lists
{
    public class AccountList : List
    {
        [JsonProperty("list_type")]
        public MediaType ListType { get; set; }
    }
}