using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents a collection of episode groups for a TV show.
/// </summary>
public class TvGroupCollection
{
    /// <summary>
    /// Gets or sets the episode group collection ID.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the episode group collection.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the type of episode grouping.
    /// </summary>
    [JsonProperty("type")]
    public TvGroupType Type { get; set; }

    /// <summary>
    /// Gets or sets the description of the episode group collection.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the network that created this grouping.
    /// </summary>
    [JsonProperty("network")]
    public NetworkWithLogo Network { get; set; }

    /// <summary>
    /// Gets or sets the total number of episodes in the collection.
    /// </summary>
    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }

    /// <summary>
    /// Gets or sets the number of groups in the collection.
    /// </summary>
    [JsonProperty("group_count")]
    public int GroupCount { get; set; }

    /// <summary>
    /// Gets or sets the list of episode groups.
    /// </summary>
    [JsonProperty("groups")]
    public List<TvGroup> Groups { get; set; }
}
