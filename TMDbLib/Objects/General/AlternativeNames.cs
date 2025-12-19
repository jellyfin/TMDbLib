using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a collection of alternative names.
/// </summary>
public class AlternativeNames
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of alternative names.
    /// </summary>
    [JsonProperty("results")]
    public List<AlternativeName> Results { get; set; }
}
