namespace TMDbLib.Objects.Movies
{
    public class TvEpisodeAccountState
    {
        /// <summary>
        /// The TMDb if for the related movie
        /// </summary>
        public int Id { get; set; }

        public double? Rating { get; set; }

        public int EpisodeNumber { get; set; }
    }
}
