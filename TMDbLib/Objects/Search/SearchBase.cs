using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Utilities;

namespace TMDbLib.Objects.Search
{
    [JsonConverter(typeof(SearchBaseConverter))]
    public class SearchBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonIgnore]
        public MediaType MediaType { get; set; }
    }
}