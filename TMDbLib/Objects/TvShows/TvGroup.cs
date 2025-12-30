using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents a group of episodes in an episode group collection.
/// </summary>
public class TvGroup
{
    /// <summary>
    /// Gets or sets the group ID.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the group.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the display order of the group.
    /// </summary>
    [JsonProperty("order")]
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the list of episodes in the group.
    /// </summary>
    [JsonProperty("episodes")]
    public List<TvGroupEpisode>? Episodes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the group is locked from editing.
    /// </summary>
    [JsonProperty("locked")]
    public bool Locked { get; set; }
}
