using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a watch provider item.
/// </summary>
public class WatchProviderItem
{
    /// <summary>
    /// Gets or sets the display priority for ordering providers.
    /// </summary>
    [JsonProperty("display_priority")]
    public int? DisplayPriority { get; set; }

    /// <summary>
    /// Gets or sets the logo path for the provider.
    /// </summary>
    [JsonProperty("logo_path")]
    public string LogoPath { get; set; }

    /// <summary>
    /// Gets or sets the provider ID.
    /// </summary>
    [JsonProperty("provider_id")]
    public int? ProviderId { get; set; }

    /// <summary>
    /// Gets or sets the provider name.
    /// </summary>
    [JsonProperty("provider_name")]
    public string ProviderName { get; set; }
}
