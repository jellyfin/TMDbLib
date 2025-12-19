namespace TMDbLib.Objects.Movies
{
    /// <summary>
    /// Represents a spoken language in a movie.
    /// </summary>
    public class SpokenLanguage
    {
        /// <summary>
        /// Gets or sets a language code, e.g. en.
        /// </summary>
        public string Iso_639_1 { get; set; }

        /// <summary>
        /// Gets or sets the name of the language.
        /// </summary>
        public string Name { get; set; }
    }
}
