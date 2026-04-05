using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Certifications
{
    /// <summary>
    /// Represents a content rating certification for movies or TV shows.
    /// </summary>
    public class CertificationItem
    {
        /// <summary>
        /// Gets or sets the certification code (e.g., "PG-13", "R", "TV-MA").
        /// </summary>
        [JsonPropertyName("certification")]
        public string? Certification { get; set; }

        /// <summary>
        /// Gets or sets the meaning or description of the certification.
        /// </summary>
        [JsonPropertyName("meaning")]
        public string? Meaning { get; set; }

        /// <summary>
        /// Gets or sets the display order of the certification.
        /// </summary>
        [JsonPropertyName("order")]
        public int Order { get; set; }
    }
}
