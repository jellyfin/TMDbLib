using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a keyword search result.
/// </summary>
public class SearchKeyword
{
    /// <summary>
    /// Gets or sets the keyword ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the keyword name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
