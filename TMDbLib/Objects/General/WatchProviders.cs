using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents watch provider information for a media item.
/// </summary>
public class WatchProviders
{
    /// <summary>
    /// Gets or sets the link to the watch providers page.
    /// </summary>
    [JsonProperty("link")]
    public string? Link { get; set; }

    /// <summary>
    /// Gets or sets the list of flat rate (subscription) providers.
    /// </summary>
    [JsonProperty("flatrate")]
    public List<WatchProviderItem>? FlatRate { get; set; }

    /// <summary>
    /// Gets or sets the list of rental providers.
    /// </summary>
    [JsonProperty("rent")]
    public List<WatchProviderItem>? Rent { get; set; }

    /// <summary>
    /// Gets or sets the list of purchase providers.
    /// </summary>
    [JsonProperty("buy")]
    public List<WatchProviderItem>? Buy { get; set; }

    /// <summary>
    /// Gets or sets the list of free providers.
    /// </summary>
    [JsonProperty("free")]
    public List<WatchProviderItem>? Free { get; set; }

    /// <summary>
    /// Gets or sets the list of ad-supported providers.
    /// </summary>
    [JsonProperty("ads")]
    public List<WatchProviderItem>? Ads { get; set; }
}
