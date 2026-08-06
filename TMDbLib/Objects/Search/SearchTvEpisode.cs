using System;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Objects.Search;

/// <summary>
/// TV episode search result.
/// </summary>
public class SearchTvEpisode : TmdbEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTvEpisode"/> class.
    /// </summary>
    public SearchTvEpisode()
    {
        MediaType = MediaType.Episode;
    }

    /// <summary>
    /// Gets or sets the air date.
    /// </summary>
    [JsonPropertyName("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonPropertyName("episode_number")]
    public int EpisodeNumber { get; set; }

    /// <summary>
    /// Gets or sets the episode type (e.g. standard, finale).
    /// </summary>
    [JsonPropertyName("episode_type")]
    public string? EpisodeType { get; set; }

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
    /// Gets or sets the parent TV show id.
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
}
