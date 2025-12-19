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
        public string Certification { get; set; }

        /// <summary>
        /// Gets or sets the meaning or description of the certification.
        /// </summary>
        public string Meaning { get; set; }

        /// <summary>
        /// Gets or sets the display order of the certification.
        /// </summary>
        public int Order { get; set; }
    }
}
