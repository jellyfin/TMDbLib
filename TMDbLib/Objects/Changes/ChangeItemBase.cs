using System;
using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Base class for all change item types.
/// </summary>
[JsonDerivedType(typeof(ChangeItemAdded))]
[JsonDerivedType(typeof(ChangeItemCreated))]
[JsonDerivedType(typeof(ChangeItemDeleted))]
[JsonDerivedType(typeof(ChangeItemDestroyed))]
[JsonDerivedType(typeof(ChangeItemUpdated))]
[JsonConverter(typeof(ChangeItemConverter))]
public abstract class ChangeItemBase
{
    /// <summary>
    /// Gets or sets the type of action that occurred.
    /// </summary>
    [JsonPropertyName("action")]
    public ChangeAction Action { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the change item.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en
    /// This field is not always set.
    /// </summary>
    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the change occurred.
    /// </summary>
    [JsonPropertyName("time")]
    [JsonConverter(typeof(TmdbUtcTimeConverterFactory))]
    public DateTime Time { get; set; }
}
