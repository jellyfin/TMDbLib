using Newtonsoft.Json;

namespace TMDbLib.Objects.Search
{
    /// <summary>
    /// Movie search result with a user rating.
    /// </summary>
    public class SearchMovieWithRating : SearchMovie
    {
        /// <summary>
        /// Gets or sets the user rating.
        /// </summary>
        [JsonProperty("rating")]
        public double Rating { get; set; }
    }
}
