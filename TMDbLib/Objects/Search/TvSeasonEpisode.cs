using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
    [JsonProperty("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the list of crew members for the episode.
    /// </summary>
    [JsonProperty("crew")]
    public List<Crew>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonProperty("episode_number")]
    public int EpisodeNumber { get; set; }

    /// <summary>
    /// Gets or sets the episode type.
    /// </summary>
    [JsonProperty("episode_type")]
    public string? EpisodeType { get; set; }

    /// <summary>
    /// Gets or sets the list of guest stars for the episode.
    /// </summary>
    [JsonProperty("guest_stars")]
    public List<Cast>? GuestStars { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID of the episode.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the episode.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview text of the episode.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the production code of the episode.
    /// </summary>
    [JsonProperty("production_code")]
    public string? ProductionCode { get; set; }

    /// <summary>
    /// Gets or sets the runtime of the episode in minutes.
    /// </summary>
    [JsonProperty("runtime")]
    public int? Runtime { get; set; }

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

    /// <summary>
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the total vote count.
    /// </summary>
    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }
}
