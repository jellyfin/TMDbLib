using System;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Find;

/// <summary>
/// Represents a TV season found through external ID search.
/// </summary>
public class FindTvSeason
{
    /// <summary>
    /// Gets or sets the air date of the season.
    /// </summary>
    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the number of episodes in the season.
    /// </summary>
    [JsonPropertyName("episode_count")]
    public int EpisodeCount { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID of the season.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the season.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview description of the season.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the poster path for the season.
    /// </summary>
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID of the parent TV show.
    /// </summary>
    [JsonPropertyName("show_id")]
    public int ShowId { get; set; }
}
