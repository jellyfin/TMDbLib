using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a TV season episode with full details.
/// </summary>
public class TvSeasonEpisode
{
    /// <summary>
    /// Gets or sets the air date of the episode.
    /// </summary>
    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the list of crew members for the episode.
    /// </summary>
    [JsonPropertyName("crew")]
    public List<Crew>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonPropertyName("episode_number")]
    public long EpisodeNumber { get; set; }

    /// <summary>
    /// Gets or sets the episode type.
    /// </summary>
    [JsonPropertyName("episode_type")]
    public string? EpisodeType { get; set; }

    /// <summary>
    /// Gets or sets the list of guest stars for the episode.
    /// </summary>
    [JsonPropertyName("guest_stars")]
    public List<Cast>? GuestStars { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID of the episode.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the episode.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview text of the episode.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the production code of the episode.
    /// </summary>
    [JsonPropertyName("production_code")]
    public string? ProductionCode { get; set; }

    /// <summary>
    /// Gets or sets the runtime of the episode in minutes.
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
