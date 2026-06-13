using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Container for alternative movie titles.
/// </summary>
public class AlternativeTitles
{
    /// <summary>
    /// Gets or sets the movie id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the alternative titles.
    /// </summary>
    [JsonPropertyName("titles")]
    public List<AlternativeTitle>? Titles { get; set; }
}
