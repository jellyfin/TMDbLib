using System;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a TV episode search result.
/// </summary>
public class SearchTvEpisode : SearchBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTvEpisode"/> class.
    /// </summary>
    public SearchTvEpisode()
    {
        MediaType = MediaType.Episode;
    }

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
    /// Gets or sets the season number.
    /// </summary>
    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }

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
}
