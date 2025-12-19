using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a container for a list of results with an associated ID.
/// </summary>
/// <typeparam name="T">The type of items contained in the results.</typeparam>
public class ResultContainer<T>
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of results.
    /// </summary>
    [JsonProperty("results")]
    public List<T> Results { get; set; }
}
