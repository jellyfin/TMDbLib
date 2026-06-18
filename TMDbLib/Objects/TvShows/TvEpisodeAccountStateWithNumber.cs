using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// TV episode account state including the episode number.
/// </summary>
public class TvEpisodeAccountStateWithNumber : TvEpisodeAccountState
{
    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonPropertyName("episode_number")]
    public int EpisodeNumber { get; set; }
}
