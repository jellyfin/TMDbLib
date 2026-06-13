using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Movie releases across countries.
/// </summary>
public class Releases
{
    /// <summary>
    /// Gets or sets the per-country release information.
    /// </summary>
    [JsonProperty("countries")]
    public List<Country>? Countries { get; set; }

    /// <summary>
    /// Gets or sets the movie id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
