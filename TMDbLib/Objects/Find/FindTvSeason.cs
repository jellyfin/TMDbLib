using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Find;

/// <summary>
/// Represents a TV season found through external ID search.
/// </summary>
public class FindTvSeason
{
    /// <summary>
    /// Gets or sets the air date of the season.
    /// </summary>
    [JsonProperty("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the number of episodes in the season.
    /// </summary>
    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID of the season.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the season.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview description of the season.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the poster path for the season.
    /// </summary>
    [JsonProperty("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonProperty("season_number")]
    public int SeasonNumber { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID of the parent TV show.
    /// </summary>
    [JsonProperty("show_id")]
    public int ShowId { get; set; }
}
