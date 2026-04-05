using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a watch provider region.
/// </summary>
public class WatchProviderRegion
{
    /// <summary>
    /// Gets or sets a country code, e.g. US.
    /// </summary>
    [JsonPropertyName("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets the English name of the region.
    /// </summary>
    [JsonPropertyName("english_name")]
    public string? EnglishName { get; set; }

    /// <summary>
    /// Gets or sets the native name of the region.
    /// </summary>
    [JsonPropertyName("native_name")]
    public string? NativeName { get; set; }
}
