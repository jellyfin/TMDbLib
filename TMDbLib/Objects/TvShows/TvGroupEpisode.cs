using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Episode within an episode group.
/// </summary>
public class TvGroupEpisode : TvEpisodeBase
{
    /// <summary>
    /// Gets or sets the order within the group.
    /// </summary>
    [JsonProperty("order")]
    public int Order { get; set; }
}
