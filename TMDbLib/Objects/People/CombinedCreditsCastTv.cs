using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents a TV show cast credit for a person.
/// </summary>
public class CombinedCreditsCastTv : CombinedCreditsCastBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CombinedCreditsCastTv"/> class.
    /// </summary>
    public CombinedCreditsCastTv()
    {
        MediaType = MediaType.Tv;
    }

    /// <summary>
    /// Gets or sets the TV show name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the original TV show name.
    /// </summary>
    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the first air date.
    /// </summary>
    [JsonPropertyName("first_air_date")]
    [JsonConverter(typeof(TmdbPartialDateConverter))]
    public DateTime? FirstAirDate { get; set; }

    /// <summary>
    /// Gets or sets the origin country codes.
    /// </summary>
    [JsonPropertyName("origin_country")]
    public List<string>? OriginCountry { get; set; }

    /// <summary>
    /// Gets or sets the number of episodes.
    /// </summary>
    [JsonPropertyName("episode_count")]
    public int EpisodeCount { get; set; }
}
