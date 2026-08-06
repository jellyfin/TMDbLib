using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Collection of episode groups for a TV show.
/// </summary>
public class TvGroupCollection
{
    /// <summary>
    /// Gets or sets the collection id.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the collection name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the episode grouping type.
    /// </summary>
    [JsonPropertyName("type")]
    public TvGroupType Type { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the network that created this grouping.
    /// </summary>
    [JsonPropertyName("network")]
    public NetworkWithLogo? Network { get; set; }

    /// <summary>
    /// Gets or sets the total number of episodes.
    /// </summary>
    [JsonPropertyName("episode_count")]
    public int EpisodeCount { get; set; }

    /// <summary>
    /// Gets or sets the number of groups.
    /// </summary>
    [JsonPropertyName("group_count")]
    public int GroupCount { get; set; }

    /// <summary>
    /// Gets or sets the episode groups.
    /// </summary>
    [JsonPropertyName("groups")]
    public List<TvGroup>? Groups { get; set; }
}
