using System;
using Newtonsoft.Json;
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
    [JsonProperty("air_date")]
    public DateTime? AirDate { get; set; }

    /// <summary>
    /// Gets or sets the number of episodes in the season.
    /// </summary>
    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }

    /// <summary>
    /// Gets or sets the name of the season.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the overview text of the season.
    /// </summary>
    [JsonProperty("overview")]
    public string Overview { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonProperty("season_number")]
    public int SeasonNumber { get; set; }
}
