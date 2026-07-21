using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Account state for a TV episode.
/// </summary>
public class TvEpisodeAccountState : TvAccountState
{
    /// <summary>
    /// Gets or sets the episode TMDb id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
