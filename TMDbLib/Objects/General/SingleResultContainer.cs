using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a container for a single result with an associated ID.
/// </summary>
/// <typeparam name="T">The type of the result item.</typeparam>
public class SingleResultContainer<T>
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the result.
    /// </summary>
    [JsonProperty("results")]
    public T Results { get; set; }
}
