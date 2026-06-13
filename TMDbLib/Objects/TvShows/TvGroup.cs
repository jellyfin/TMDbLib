using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Group of episodes in an episode group collection.
/// </summary>
public class TvGroup
{
    /// <summary>
    /// Gets or sets the group id.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the group name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the display order.
    /// </summary>
    [JsonPropertyName("order")]
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the episodes in the group.
    /// </summary>
    [JsonPropertyName("episodes")]
    public List<TvGroupEpisode>? Episodes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the group is locked from editing.
    /// </summary>
    [JsonPropertyName("locked")]
    public bool Locked { get; set; }
}
