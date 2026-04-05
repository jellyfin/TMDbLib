using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a collection of poster images.
/// </summary>
public class PosterImages
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of poster images.
    /// </summary>
    [JsonPropertyName("posters")]
    public List<ImageData>? Posters { get; set; }
}
