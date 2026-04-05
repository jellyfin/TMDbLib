using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a collection of images for a media item.
/// </summary>
[JsonDerivedType(typeof(ImagesWithId))]
public class Images
{
    /// <summary>
    /// Gets or sets the list of backdrop images.
    /// </summary>
    [JsonPropertyName("backdrops")]
    public List<ImageData>? Backdrops { get; set; }

    /// <summary>
    /// Gets or sets the list of poster images.
    /// </summary>
    [JsonPropertyName("posters")]
    public List<ImageData>? Posters { get; set; }

    /// <summary>
    /// Gets or sets the list of logo images.
    /// </summary>
    [JsonPropertyName("logos")]
    public List<ImageData>? Logos { get; set; }
}
