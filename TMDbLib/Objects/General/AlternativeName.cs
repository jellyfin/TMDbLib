using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents an alternative name for a person or entity.
/// </summary>
public class AlternativeName
{
    /// <summary>
    /// Gets or sets the alternative name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the alternative name.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
