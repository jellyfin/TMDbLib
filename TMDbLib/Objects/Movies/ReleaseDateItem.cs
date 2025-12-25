using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Represents a specific movie release date entry.
/// </summary>
public class ReleaseDateItem
{
    /// <summary>
    /// Gets or sets the certification rating.
    /// </summary>
    [JsonProperty("certification")]
    public string? Certification { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonProperty("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets additional notes about the release.
    /// </summary>
    [JsonProperty("note")]
    public string? Note { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonProperty("release_date")]
    [JsonConverter(typeof(IsoDateTimeConverter))]
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the release type.
    /// </summary>
    [JsonProperty("type")]
    public ReleaseDateType Type { get; set; }
}
