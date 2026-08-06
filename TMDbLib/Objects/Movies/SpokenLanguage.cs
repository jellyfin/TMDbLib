using System.Text.Json.Serialization;

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
        [JsonPropertyName("english_name")]
        public string? EnglishName { get; set; }

        /// <summary>
        /// Gets or sets the language code, e.g. en.
        /// </summary>
        [JsonPropertyName("iso_639_1")]
        public string? Iso_639_1 { get; set; }

        /// <summary>
        /// Gets or sets the language name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
