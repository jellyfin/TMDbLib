using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Represents a container for movie keywords.
/// </summary>
public class KeywordsContainer
{
    /// <summary>
    /// Gets or sets the movie ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of keywords associated with the movie.
    /// </summary>
    [JsonPropertyName("keywords")]
    public List<Keyword>? Keywords { get; set; }
}
