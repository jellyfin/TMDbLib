using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a collection of poster images.
/// </summary>
public class PosterImages
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of poster images.
    /// </summary>
    [JsonProperty("posters")]
    public List<ImageData> Posters { get; set; }
}
