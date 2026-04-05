using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a paginated container for search results.
/// </summary>
/// <typeparam name="T">The type of items contained in the search results.</typeparam>
public class SearchContainer<T>
{
    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the list of results.
    /// </summary>
    [JsonPropertyName("results")]
    public List<T>? Results { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total number of results.
    /// </summary>
    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }
}
