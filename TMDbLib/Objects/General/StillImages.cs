using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a collection of still images.
/// </summary>
public class StillImages
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of still images.
    /// </summary>
    [JsonProperty("stills")]
    public List<ImageData>? Stills { get; set; }
}
