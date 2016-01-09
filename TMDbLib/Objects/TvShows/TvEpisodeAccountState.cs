using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    public class TvEpisodeAccountState
    {
        /// <summary>
        /// The TMDb if for the related movie
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("rating")]
        public double? Rating { get; set; }

        [JsonProperty("episode_number")]
        public int EpisodeNumber { get; set; }
    }
}
