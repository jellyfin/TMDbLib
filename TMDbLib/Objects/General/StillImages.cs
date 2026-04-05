using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a collection of still images.
/// </summary>
public class StillImages
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of still images.
    /// </summary>
    [JsonPropertyName("stills")]
    public List<ImageData>? Stills { get; set; }
}
