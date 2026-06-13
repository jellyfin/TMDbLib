using System.Collections.Generic;
using Newtonsoft.Json;
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
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the keywords.
    /// </summary>
    [JsonProperty("keywords")]
    public List<Keyword>? Keywords { get; set; }
}
