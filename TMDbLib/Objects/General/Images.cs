using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a collection of images for a media item.
/// </summary>
public class Images
{
    /// <summary>
    /// Gets or sets the list of backdrop images.
    /// </summary>
    [JsonProperty("backdrops")]
    public List<ImageData>? Backdrops { get; set; }

    /// <summary>
    /// Gets or sets the list of poster images.
    /// </summary>
    [JsonProperty("posters")]
    public List<ImageData>? Posters { get; set; }

    /// <summary>
    /// Gets or sets the list of logo images.
    /// </summary>
    [JsonProperty("logos")]
    public List<ImageData>? Logos { get; set; }
}
