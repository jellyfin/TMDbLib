using Newtonsoft.Json;

namespace TMDbLib.Objects.Countries;

/// <summary>
/// Represents a country with its ISO code and localized names.
/// </summary>
public class Country
{
    /// <summary>
    /// Gets or sets the ISO 3166-1 country code.
    /// </summary>
    [JsonProperty("iso_3166_1")]
    public string Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets the English name of the country.
    /// </summary>
    [JsonProperty("english_name")]
    public string EnglishName { get; set; }

    /// <summary>
    /// Gets or sets the native name of the country.
    /// </summary>
    [JsonProperty("native_name")]
    public string NativeName { get; set; }
}
