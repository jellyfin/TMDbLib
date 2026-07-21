using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Objects.General;

/// <summary>
/// Aggregated crew information across multiple episodes or projects.
/// </summary>
public class CrewAggregate : TmdbPersonSummary
{
    /// <summary>
    /// Gets or sets the crew department.
    /// </summary>
    [JsonProperty("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the jobs performed by the crew member.
    /// </summary>
    [JsonProperty("jobs")]
    public List<CrewJob>? Jobs { get; set; }

    /// <summary>
    /// Gets or sets the total number of episodes the crew member worked on.
    /// </summary>
    [JsonProperty("total_episode_count")]
    public int TotalEpisodeCount { get; set; }
}
