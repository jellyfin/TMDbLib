using System;
using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Objects.Search;

/// <summary>
/// TV season search result.
/// </summary>
public class SearchTvSeason : TmdbEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTvSeason"/> class.
    /// </summary>
    public SearchTvSeason()
    {
        MediaType = MediaType.Season;
    }

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
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }
}
