using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a collection of alternative names.
/// </summary>
public class AlternativeNames
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of alternative names.
    /// </summary>
    [JsonPropertyName("results")]
    public List<AlternativeName>? Results { get; set; }
}
