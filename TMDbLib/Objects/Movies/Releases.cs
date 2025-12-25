using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Represents movie releases across different countries.
/// </summary>
public class Releases
{
    /// <summary>
    /// Gets or sets the list of country-specific release information.
    /// </summary>
    [JsonProperty("countries")]
    public List<Country>? Countries { get; set; }

    /// <summary>
    /// Gets or sets the movie ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
