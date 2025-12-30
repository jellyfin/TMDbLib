using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

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
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the original TV show name.
    /// </summary>
    [JsonProperty("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the first air date.
    /// </summary>
    [JsonProperty("first_air_date")]
    public DateTime? FirstAirDate { get; set; }

    /// <summary>
    /// Gets or sets the origin country codes.
    /// </summary>
    [JsonProperty("origin_country")]
    public List<string>? OriginCountry { get; set; }

    /// <summary>
    /// Gets or sets the number of episodes.
    /// </summary>
    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }
}
