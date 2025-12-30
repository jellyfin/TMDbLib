using System.Collections.Generic;
using Newtonsoft.Json;
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
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of keywords associated with the movie.
    /// </summary>
    [JsonProperty("keywords")]
    public List<Keyword>? Keywords { get; set; }
}
