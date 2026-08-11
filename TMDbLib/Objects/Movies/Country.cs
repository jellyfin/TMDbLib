using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Country-specific movie release information.
/// </summary>
public class Country
{
    /// <summary>
    /// Gets or sets the certification rating.
    /// </summary>
    [JsonPropertyName("certification")]
    public string? Certification { get; set; }

    /// <summary>
    /// Gets or sets the release descriptors.
    /// </summary>
    [JsonPropertyName("descriptors")]
    public List<string>? Descriptors { get; set; }

    /// <summary>
    /// Gets or sets the country code, e.g. US.
    /// </summary>
    [JsonPropertyName("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is the primary release country.
    /// </summary>
    [JsonPropertyName("primary")]
    public bool Primary { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonPropertyName("release_date")]
    public DateTime? ReleaseDate { get; set; }
}
