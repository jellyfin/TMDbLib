using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Base class for TV episode information.
/// </summary>
public class TvEpisodeBase : TvEpisodeInfo
{
    /// <summary>
    /// Gets or sets the air date of the episode.
    /// </summary>
    [JsonProperty("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the episode.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the overview text of the episode.
    /// </summary>
    [JsonProperty("overview")]
    public string Overview { get; set; }

    /// <summary>
    /// Gets or sets the production code of the episode.
    /// </summary>
    [JsonProperty("production_code")]
    public string ProductionCode { get; set; }

    /// <summary>
    /// Gets or sets the TV show ID.
    /// </summary>
    [JsonProperty("show_id")]
    public int ShowId { get; set; }

    /// <summary>
    /// Gets or sets the still image path.
    /// </summary>
    [JsonProperty("still_path")]
    public string StillPath { get; set; }

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

    /// <summary>
    /// Gets or sets the runtime of the episode in minutes.
    /// </summary>
    [JsonProperty("runtime")]
    public int? Runtime { get; set; }

    /// <summary>
    /// Gets or sets the episode type.
    /// </summary>
    [JsonProperty("episode_type")]
    public string EpisodeType { get; set; }
}
