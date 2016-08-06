using Newtonsoft.Json;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Movies
{
    [JsonConverter(typeof(AccountStateConverter))]
    public class TvEpisodeAccountState
    {
        [JsonProperty("episode_number")]
        public int EpisodeNumber { get; set; }

        /// <summary>
        /// The TMDb if for the related movie
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("rating")]
        public double? Rating { get; set; }
    }
}
