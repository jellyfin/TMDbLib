using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// TV season in a credit context.
/// </summary>
public class CreditSeason
{
    /// <summary>
    /// Gets or sets the air date.
    /// </summary>
    [JsonProperty("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the number of episodes.
    /// </summary>
    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }

    /// <summary>
    /// Gets or sets the season id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the season name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonProperty("season_number")]
    public int SeasonNumber { get; set; }

    /// <summary>
    /// Gets or sets the parent TV show id.
    /// </summary>
    [JsonProperty("show_id")]
    public int ShowId { get; set; }
}
