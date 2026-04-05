using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Lists;

/// <summary>
/// Represents the status of an item in a list.
/// </summary>
public class ListStatus
{
    /// <summary>
    /// Gets or sets the list ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is present in the list.
    /// </summary>
    [JsonPropertyName("item_present")]
    public bool ItemPresent { get; set; }
}
