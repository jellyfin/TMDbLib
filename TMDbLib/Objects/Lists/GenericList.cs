using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.Lists;

/// <summary>
/// Represents a generic TMDb list with search results.
/// </summary>
public class GenericList : TMDbList<string>
{
    /// <summary>
    /// Gets or sets the username of the list creator.
    /// </summary>
    [JsonProperty("created_by")]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the list of items in the list.
    /// </summary>
    [JsonProperty("items")]
    public List<SearchBase> Items { get; set; }
}
