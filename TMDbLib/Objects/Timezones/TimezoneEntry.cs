using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Timezones;

/// <summary>
/// Represents a single TMDb configuration/timezones entry pairing an ISO 3166-1 country code with its zones.
/// </summary>
public class TimezoneEntry
{
    /// <summary>
    /// Gets or sets the ISO 3166-1 country code.
    /// </summary>
    [JsonPropertyName("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets the IANA timezone identifiers for the country.
    /// </summary>
    [JsonPropertyName("zones")]
    public List<string>? Zones { get; set; }
}
