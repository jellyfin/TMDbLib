using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a watch provider item.
/// </summary>
public class WatchProviderItem
{
    /// <summary>
    /// Gets or sets the display priority for ordering providers.
    /// </summary>
    [JsonPropertyName("display_priority")]
    public int? DisplayPriority { get; set; }

    /// <summary>
    /// Gets or sets the logo path for the provider.
    /// </summary>
    [JsonPropertyName("logo_path")]
    public string? LogoPath { get; set; }

    /// <summary>
    /// Gets or sets the provider ID.
    /// </summary>
    [JsonPropertyName("provider_id")]
    public int? ProviderId { get; set; }

    /// <summary>
    /// Gets or sets the provider name.
    /// </summary>
    [JsonPropertyName("provider_name")]
    public string? ProviderName { get; set; }
}
