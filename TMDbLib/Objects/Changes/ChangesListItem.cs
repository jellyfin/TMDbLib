using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Represents an item in a list of changes.
/// </summary>
public class ChangesListItem
{
    /// <summary>
    /// Gets or sets a value indicating whether the item is marked as adult content.
    /// </summary>
    [JsonPropertyName("adult")]
    public bool? Adult { get; set; }

    /// <summary>
    /// Gets or sets the ID of the changed item.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonConverter(typeof(TmdbNullIntAsZero))]
    public int Id { get; set; }
}
