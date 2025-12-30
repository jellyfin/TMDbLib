namespace TMDbLib.Objects.General
{
    /// <summary>
    /// Represents a production country.
    /// </summary>
    public class ProductionCountry
    {
        /// <summary>
        /// Gets or sets a country code, e.g. US.
        /// </summary>
        public string? Iso_3166_1 { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string? Name { get; set; }
    }
}
