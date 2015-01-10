namespace TMDbLib.Objects.TvShows
{
    public class ContentRating
    {
        /// <summary>
        /// The country iso code specified by the user. Ex. US
        /// </summary>
        public string Iso_3166_1 { get; set; }

        public string Rating { get; set; }
    }
}
