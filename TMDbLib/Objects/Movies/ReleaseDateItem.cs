using System;
using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Represents a specific movie release date entry.
/// </summary>
public class ReleaseDateItem
{
    /// <summary>
    /// Gets or sets the certification rating.
    /// </summary>
    [JsonPropertyName("certification")]
    public string? Certification { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets additional notes about the release.
    /// </summary>
    [JsonPropertyName("note")]
    public string? Note { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonPropertyName("release_date")]
    [JsonConverter(typeof(IsoDateTimeConverterFactory))]
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the release type.
    /// </summary>
    [JsonPropertyName("type")]
    public ReleaseDateType Type { get; set; }
}
