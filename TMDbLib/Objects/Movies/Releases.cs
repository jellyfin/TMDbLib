using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Movie releases across countries.
/// </summary>
public class Releases
{
    /// <summary>
    /// Gets or sets the per-country release information.
    /// </summary>
    [JsonPropertyName("countries")]
    public List<Country>? Countries { get; set; }

    /// <summary>
    /// Gets or sets the movie id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
