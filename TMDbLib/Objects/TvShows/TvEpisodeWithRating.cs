namespace TMDbLib.Objects.TvShows
{
    /// <summary>
    /// TV episode with a user rating.
    /// </summary>
    public class TvEpisodeWithRating : TvEpisode
    {
        /// <summary>
        /// Gets or sets the user rating.
        /// </summary>
        public double Rating { get; set; }
    }
}
