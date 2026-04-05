using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents the account state for a TV episode including the episode number.
/// </summary>
public class TvEpisodeAccountStateWithNumber : TvEpisodeAccountState
{
    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonPropertyName("episode_number")]
    public long EpisodeNumber { get; set; }
}
