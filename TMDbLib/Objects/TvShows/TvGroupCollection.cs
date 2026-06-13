using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Collection of episode groups for a TV show.
/// </summary>
public class TvGroupCollection
{
    /// <summary>
    /// Gets or sets the collection id.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the collection name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the episode grouping type.
    /// </summary>
    [JsonProperty("type")]
    public TvGroupType Type { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the network that created this grouping.
    /// </summary>
    [JsonProperty("network")]
    public NetworkWithLogo? Network { get; set; }

    /// <summary>
    /// Gets or sets the total number of episodes.
    /// </summary>
    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }

    /// <summary>
    /// Gets or sets the number of groups.
    /// </summary>
    [JsonProperty("group_count")]
    public int GroupCount { get; set; }

    /// <summary>
    /// Gets or sets the episode groups.
    /// </summary>
    [JsonProperty("groups")]
    public List<TvGroup>? Groups { get; set; }
}
