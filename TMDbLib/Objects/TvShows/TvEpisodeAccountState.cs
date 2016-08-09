using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows
{
    public class TvEpisodeAccountState : TvAccountState
    {
        [JsonProperty("episode_number")]
        public int EpisodeNumber { get; set; }

        /// <summary>
        /// The TMDb if for the related movie
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
