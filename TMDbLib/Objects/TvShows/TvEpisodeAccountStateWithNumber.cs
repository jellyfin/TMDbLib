using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents the account state for a TV episode including the episode number.
/// </summary>
public class TvEpisodeAccountStateWithNumber : TvEpisodeAccountState
{
    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonProperty("episode_number")]
    public int EpisodeNumber { get; set; }
}
