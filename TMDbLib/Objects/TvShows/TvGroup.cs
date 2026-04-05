using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents a group of episodes in an episode group collection.
/// </summary>
public class TvGroup
{
    /// <summary>
    /// Gets or sets the group ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the group.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the display order of the group.
    /// </summary>
    [JsonPropertyName("order")]
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the list of episodes in the group.
    /// </summary>
    [JsonPropertyName("episodes")]
    public List<TvGroupEpisode>? Episodes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the group is locked from editing.
    /// </summary>
    [JsonPropertyName("locked")]
    public bool Locked { get; set; }
}
