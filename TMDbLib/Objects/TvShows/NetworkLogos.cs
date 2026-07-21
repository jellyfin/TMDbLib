using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Logos for a TV network.
/// </summary>
public class NetworkLogos
{
    /// <summary>
    /// Gets or sets the network id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the logo images.
    /// </summary>
    [JsonPropertyName("logos")]
    public List<ImageData>? Logos { get; set; }
}
