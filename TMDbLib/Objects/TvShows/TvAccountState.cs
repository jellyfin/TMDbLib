using Newtonsoft.Json;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.TvShows
{
    [JsonConverter(typeof(AccountStateConverter))]
    public class TvAccountState
    {
        [JsonProperty("rating")]
        public double? Rating { get; set; }
    }
}