using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a TV show that a person is known for.
/// </summary>
public class KnownForTv : KnownForBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KnownForTv"/> class.
    /// </summary>
    public KnownForTv()
    {
        MediaType = MediaType.Tv;
    }

    /// <summary>
    /// Gets or sets the first air date of the TV show.
    /// </summary>
    [JsonPropertyName("first_air_date")]
    [JsonConverter(typeof(TmdbPartialDateConverter))]

    public DateTime? FirstAirDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the TV show.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the original name of the TV show.
    /// </summary>
    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the list of origin country codes.
    /// </summary>
    [JsonPropertyName("origin_country")]
    public List<string>? OriginCountry { get; set; }
}
