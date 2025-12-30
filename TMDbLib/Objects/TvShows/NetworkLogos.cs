using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents a collection of logos for a TV network.
/// </summary>
public class NetworkLogos
{
    /// <summary>
    /// Gets or sets the network ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of logo images.
    /// </summary>
    [JsonProperty("logos")]
    public List<ImageData>? Logos { get; set; }
}
