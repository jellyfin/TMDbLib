using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Represents a collection of change items for a specific property.
/// </summary>
public class Change
{
    /// <summary>
    /// Gets or sets the list of change items.
    /// </summary>
    [JsonProperty("items")]
    public List<ChangeItemBase> Items { get; set; }

    /// <summary>
    /// Gets or sets the key identifying which property was changed.
    /// </summary>
    [JsonProperty("key")]
    public string Key { get; set; }
}
