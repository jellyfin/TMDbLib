namespace TMDbLib.Objects.TvShows
{
    /// <summary>
    /// Represents a content rating for a TV show in a specific country.
    /// </summary>
    public class ContentRating
    {
        /// <summary>
        /// Gets or sets a country code, e.g. US.
        /// </summary>
        public string? Iso_3166_1 { get; set; }

        /// <summary>
        /// Gets or sets the content rating value.
        /// </summary>
        public string? Rating { get; set; }
    }
}
