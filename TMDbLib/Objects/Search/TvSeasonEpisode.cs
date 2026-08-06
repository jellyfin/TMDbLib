using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Objects.Search;

/// <summary>
/// TV season episode with full details.
/// </summary>
public class TvSeasonEpisode
{
    /// <summary>
    /// Gets or sets the air date.
    /// </summary>
    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the crew members.
    /// </summary>
    [JsonPropertyName("crew")]
    public List<Crew>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonPropertyName("episode_number")]
    public int EpisodeNumber { get; set; }

    /// <summary>
    /// Gets or sets the episode type.
    /// </summary>
    [JsonPropertyName("episode_type")]
    public string? EpisodeType { get; set; }

    /// <summary>
    /// Gets or sets the guest stars.
    /// </summary>
    [JsonPropertyName("guest_stars")]
    public List<Cast>? GuestStars { get; set; }

    /// <summary>
    /// Gets or sets the TMDb id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the episode name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the production code.
    /// </summary>
    [JsonPropertyName("production_code")]
    public string? ProductionCode { get; set; }

    /// <summary>
    /// Gets or sets the runtime in minutes.
    /// </summary>
    [JsonPropertyName("runtime")]
    public int? Runtime { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }

    /// <summary>
    /// Gets or sets the still image path.
    /// </summary>
    [JsonPropertyName("still_path")]
    public string? StillPath { get; set; }

    /// <summary>
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the total vote count.
    /// </summary>
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}
