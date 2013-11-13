namespace TMDbLib.Objects.Movies
{
    public class MovieAccountState
    {
        /// <summary>
        /// The TMDb if for the related movie
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the current favorite status of the related movie for the current user session.
        /// </summary>
        public bool Favorite { get; set; }

        /// <summary>
        /// Represents the presence of the related movie on the current user's watchlist.
        /// </summary>
        public bool Watchlist { get; set; }

        public double? Rating { get; set; }
    }
}
