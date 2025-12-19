namespace TMDbLib.Objects.Search
{
    /// <summary>
    /// Represents a movie search result with a user rating.
    /// </summary>
    public class SearchMovieWithRating : SearchMovie
    {
        /// <summary>
        /// Gets or sets the user rating for the movie.
        /// </summary>
        public double Rating { get; set; }
    }
}
