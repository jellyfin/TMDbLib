using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Represents movie releases across different countries.
/// </summary>
public class Releases
{
    /// <summary>
    /// Gets or sets the list of country-specific release information.
    /// </summary>
    [JsonPropertyName("countries")]
    public List<Country>? Countries { get; set; }

    /// <summary>
    /// Gets or sets the movie ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
