using Newtonsoft.Json;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a keyword search result.
/// </summary>
public class SearchKeyword
{
    /// <summary>
    /// Gets or sets the keyword ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the keyword name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }
}
