using System;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a TV season search result.
/// </summary>
public class SearchTvSeason : SearchBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTvSeason"/> class.
    /// </summary>
    public SearchTvSeason()
    {
        MediaType = MediaType.Season;
    }

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
    /// Gets or sets the name of the season.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the overview text of the season.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }
}
