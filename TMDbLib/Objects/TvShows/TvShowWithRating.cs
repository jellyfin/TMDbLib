using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows
{
    public class TvShowWithRating : TvShow
    {
        [JsonProperty("rating")]
        public double Rating { get; set; }
    }
}