using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Basic identifying information for a TV episode.
/// </summary>
public class TvEpisodeInfo
{
    /// <summary>
    /// Gets or sets the episode id.
    /// </summary>
    [JsonProperty("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonProperty("season_number")]
    public int SeasonNumber { get; set; }

    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonProperty("episode_number")]
    public int EpisodeNumber { get; set; }
}
