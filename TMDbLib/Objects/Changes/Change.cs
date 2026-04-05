using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Represents a collection of change items for a specific property.
/// </summary>
public class Change
{
    /// <summary>
    /// Gets or sets the list of change items.
    /// </summary>
    [JsonPropertyName("items")]
    public List<ChangeItemBase>? Items { get; set; }

    /// <summary>
    /// Gets or sets the key identifying which property was changed.
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }
}
