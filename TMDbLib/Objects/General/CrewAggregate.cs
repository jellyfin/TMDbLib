using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents aggregated crew information across multiple episodes or projects.
/// </summary>
public class CrewAggregate : CrewBase
{
    /// <summary>
    /// Gets or sets the list of jobs performed by the crew member.
    /// </summary>
    [JsonProperty("jobs")]
    public List<CrewJob> Jobs { get; set; }

    /// <summary>
    /// Gets or sets the total number of episodes the crew member worked on.
    /// </summary>
    [JsonProperty("total_episode_count")]
    public int TotalEpisodeCount { get; set; }
}
