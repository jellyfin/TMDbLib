using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a keyword or tag.
/// </summary>
public class Keyword
{
    /// <summary>
    /// Gets or sets the TMDb ID of the keyword.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the keyword.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
