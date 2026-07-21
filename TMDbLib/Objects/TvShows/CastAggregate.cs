using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Aggregated cast information across multiple episodes.
/// </summary>
public class CastAggregate : TmdbPersonSummary
{
    /// <summary>
    /// Gets or sets the roles played by the cast member.
    /// </summary>
    [JsonProperty("roles")]
    public List<CastRole>? Roles { get; set; }

    /// <summary>
    /// Gets or sets the total number of episodes the cast member appeared in.
    /// </summary>
    [JsonProperty("total_episode_count")]
    public int TotalEpisodeCount { get; set; }
}
