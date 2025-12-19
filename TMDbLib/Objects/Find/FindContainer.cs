using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.Find;

/// <summary>
/// Represents a container for find results from external ID searches.
/// </summary>
public class FindContainer
{
    /// <summary>
    /// Gets or sets the list of movie results.
    /// </summary>
    [JsonProperty("movie_results")]
    public List<SearchMovie> MovieResults { get; set; }

    /// <summary>
    /// Gets or sets the list of person results.
    /// </summary>
    [JsonProperty("person_results")]
    public List<FindPerson> PersonResults { get; set; } // Unconfirmed type

    /// <summary>
    /// Gets or sets the list of TV episode results.
    /// </summary>
    [JsonProperty("tv_episode_results")]
    public List<SearchTvEpisode> TvEpisode { get; set; }

    /// <summary>
    /// Gets or sets the list of TV show results.
    /// </summary>
    [JsonProperty("tv_results")]
    public List<SearchTv> TvResults { get; set; }

    /// <summary>
    /// Gets or sets the list of TV season results.
    /// </summary>
    [JsonProperty("tv_season_results")]
    public List<FindTvSeason> TvSeason { get; set; }
}
