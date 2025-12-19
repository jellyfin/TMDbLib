using System.Collections.Generic;
using Newtonsoft.Json;
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
    [JsonProperty("genres")]
    public List<Genre> Genres { get; set; }
}
