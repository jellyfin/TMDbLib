using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// Represents a TV show episode in a credit context.
/// </summary>
public class CreditEpisode
{
    /// <summary>
    /// Gets or sets the air date of the episode.
    /// </summary>
    [JsonProperty("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonProperty("episode_number")]
    public int EpisodeNumber { get; set; }

    /// <summary>
    /// Gets or sets the name of the episode.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview of the episode.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the season number the episode belongs to.
    /// </summary>
    [JsonProperty("season_number")]
    public int SeasonNumber { get; set; }

    /// <summary>
    /// Gets or sets the still image path for the episode.
    /// </summary>
    [JsonProperty("still_path")]
    public string? StillPath { get; set; }
}
