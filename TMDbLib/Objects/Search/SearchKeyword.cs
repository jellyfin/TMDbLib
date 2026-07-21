using Newtonsoft.Json;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Keyword search result.
/// </summary>
public class SearchKeyword
{
    /// <summary>
    /// Gets or sets the keyword id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the keyword name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }
}
