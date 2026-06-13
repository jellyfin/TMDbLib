using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Country-specific movie release information.
/// </summary>
public class Country
{
    /// <summary>
    /// Gets or sets the certification rating.
    /// </summary>
    [JsonProperty("certification")]
    public string? Certification { get; set; }

    /// <summary>
    /// Gets or sets the release descriptors.
    /// </summary>
    [JsonProperty("descriptors")]
    public List<string>? Descriptors { get; set; }

    /// <summary>
    /// Gets or sets the country code, e.g. US.
    /// </summary>
    [JsonProperty("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is the primary release country.
    /// </summary>
    [JsonProperty("primary")]
    public bool Primary { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonProperty("release_date")]
    public DateTime? ReleaseDate { get; set; }
}
