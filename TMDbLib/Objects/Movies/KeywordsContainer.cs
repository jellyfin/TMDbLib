using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Container for movie keywords.
/// </summary>
public class KeywordsContainer
{
    /// <summary>
    /// Gets or sets the movie id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the keywords.
    /// </summary>
    [JsonPropertyName("keywords")]
    public List<Keyword>? Keywords { get; set; }
}
