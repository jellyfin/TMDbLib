using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Represents a container for alternative movie titles.
/// </summary>
public class AlternativeTitles
{
    /// <summary>
    /// Gets or sets the movie ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of alternative titles.
    /// </summary>
    [JsonProperty("titles")]
    public List<AlternativeTitle> Titles { get; set; }
}
