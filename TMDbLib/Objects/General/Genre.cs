using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a genre.
/// </summary>
public class Genre
{
    /// <summary>
    /// Gets or sets the TMDb ID of the genre.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the genre.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
