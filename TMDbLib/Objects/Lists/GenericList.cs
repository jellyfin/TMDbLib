using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.Lists;

/// <summary>
/// Represents a generic TMDb list with search results.
/// </summary>
public class GenericList : TMDbList<int>
{
    /// <summary>
    /// Gets or sets the username of the list creator.
    /// </summary>
    [JsonPropertyName("created_by")]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the list of items in the list.
    /// </summary>
    [JsonPropertyName("items")]
    public List<SearchBase>? Items { get; set; }
}
