using System;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// Represents a TV show episode in a credit context.
/// </summary>
public class CreditEpisode
{
    /// <summary>
    /// Gets or sets the air date of the episode.
    /// </summary>
    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonPropertyName("episode_number")]
    public long EpisodeNumber { get; set; }

    /// <summary>
    /// Gets or sets the name of the episode.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview of the episode.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the season number the episode belongs to.
    /// </summary>
    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }

    /// <summary>
    /// Gets or sets the still image path for the episode.
    /// </summary>
    [JsonPropertyName("still_path")]
    public string? StillPath { get; set; }
}
