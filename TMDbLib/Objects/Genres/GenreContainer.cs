using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Genres;

/// <summary>
/// Represents a container for a list of genres.
/// </summary>
public class GenreContainer
{
    /// <summary>
    /// Gets or sets the list of genres.
    /// </summary>
    [JsonPropertyName("genres")]
    public List<Genre>? Genres { get; set; }
}
