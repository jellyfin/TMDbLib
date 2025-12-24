using System;
using Newtonsoft.Json;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Base class for all change item types.
/// </summary>
public abstract class ChangeItemBase
{
    /// <summary>
    /// Gets or sets the type of action that occurred.
    /// </summary>
    [JsonProperty("action")]
    public ChangeAction Action { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the change item.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en
    /// This field is not always set.
    /// </summary>
    [JsonProperty("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the change occurred.
    /// </summary>
    [JsonProperty("time")]
    [JsonConverter(typeof(TmdbUtcTimeConverter))]
    public DateTime Time { get; set; }
}
