using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Timezones;

/// <summary>
/// Represents a collection of timezones organized by country.
/// </summary>
public class Timezones
{
    /// <summary>
    /// Gets or sets the dictionary of timezones, where the key is the country code and the value is a list of timezone identifiers.
    /// </summary>
    [JsonPropertyName("list")]
    public Dictionary<string, List<string>>? List { get; set; }
}
