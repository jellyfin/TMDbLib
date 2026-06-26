using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    /// <summary>
    /// Spoken language in a movie.
    /// </summary>
    public class SpokenLanguage
    {
        /// <summary>
        /// Gets or sets the English name.
        /// </summary>
        [JsonProperty("english_name")]
        public string? EnglishName { get; set; }

        /// <summary>
        /// Gets or sets the language code, e.g. en.
        /// </summary>
        [JsonProperty("iso_639_1")]
        public string? Iso_639_1 { get; set; }

        /// <summary>
        /// Gets or sets the language name.
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
