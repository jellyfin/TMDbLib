using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Countries;

/// <summary>
/// Represents a country with its ISO code and localized names.
/// </summary>
public class Country
{
    /// <summary>
    /// Gets or sets the ISO 3166-1 country code.
    /// </summary>
    [JsonPropertyName("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets the English name of the country.
    /// </summary>
    [JsonPropertyName("english_name")]
    public string? EnglishName { get; set; }

    /// <summary>
    /// Gets or sets the native name of the country.
    /// </summary>
    [JsonPropertyName("native_name")]
    public string? NativeName { get; set; }
}
