using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// TV episode in a credit context.
/// </summary>
public class CreditEpisode
{
    /// <summary>
    /// Gets or sets the air date.
    /// </summary>
    [JsonProperty("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonProperty("episode_number")]
    public int EpisodeNumber { get; set; }

    /// <summary>
    /// Gets or sets the episode name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonProperty("season_number")]
    public int SeasonNumber { get; set; }

    /// <summary>
    /// Gets or sets the still image path.
    /// </summary>
    [JsonProperty("still_path")]
    public string? StillPath { get; set; }
}
