using System.Collections.Generic;
using Newtonsoft.Json;
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
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the logo images.
    /// </summary>
    [JsonProperty("logos")]
    public List<ImageData>? Logos { get; set; }
}
