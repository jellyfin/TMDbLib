using System;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Base class for TV episode information.
/// </summary>
public class TvEpisodeBase : TvEpisodeInfo
{
    /// <summary>
    /// Gets or sets the air date of the episode.
    /// </summary>
    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

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
    /// Gets or sets the TV show ID.
    /// </summary>
    [JsonPropertyName("show_id")]
    public int ShowId { get; set; }

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

    /// <summary>
    /// Gets or sets the runtime of the episode in minutes.
    /// </summary>
    [JsonPropertyName("runtime")]
    public int? Runtime { get; set; }

    /// <summary>
    /// Gets or sets the episode type.
    /// </summary>
    [JsonPropertyName("episode_type")]
    public string? EpisodeType { get; set; }
}
