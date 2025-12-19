namespace TMDbLib.Objects.TvShows
{
    /// <summary>
    /// Represents a TV episode with a user rating.
    /// </summary>
    public class TvEpisodeWithRating : TvEpisode
    {
        /// <summary>
        /// Gets or sets the user rating for the episode.
        /// </summary>
        public double Rating { get; set; }
    }
}
