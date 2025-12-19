using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents an episode within an episode group.
/// </summary>
public class TvGroupEpisode : TvEpisodeBase
{
    /// <summary>
    /// Gets or sets the order of the episode within the group.
    /// </summary>
    [JsonProperty("order")]
    public int Order { get; set; }
}
