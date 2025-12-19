using Newtonsoft.Json;

namespace TMDbLib.Objects.Lists;

/// <summary>
/// Represents the status of an item in a list.
/// </summary>
public class ListStatus
{
    /// <summary>
    /// Gets or sets the list ID.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is present in the list.
    /// </summary>
    [JsonProperty("item_present")]
    public bool ItemPresent { get; set; }
}
