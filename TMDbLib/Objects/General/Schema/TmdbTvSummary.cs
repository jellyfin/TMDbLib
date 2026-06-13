using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General.Schema;

/// <summary>
/// TV-shaped media summary: adds the TV-only fields on top of <see cref="TmdbMediaSummary"/>.
/// </summary>
public class TmdbTvSummary : TmdbMediaSummary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TmdbTvSummary"/> class
    /// with <see cref="TmdbEntity.MediaType"/> set to <see cref="MediaType.Tv"/>.
    /// </summary>
    public TmdbTvSummary()
    {
        MediaType = MediaType.Tv;
    }

    /// <summary>
    /// Gets or sets the first-air date.
    /// </summary>
    [JsonPropertyName("first_air_date")]
    public DateTime? FirstAirDate { get; set; }

    /// <summary>
    /// Gets or sets the show name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the origin country ISO 3166-1 codes.
    /// </summary>
    [JsonPropertyName("origin_country")]
    public List<string>? OriginCountry { get; set; }

    /// <summary>
    /// Gets or sets the original (untranslated) show name.
    /// </summary>
    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }
}
