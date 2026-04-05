using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents basic identifying information for a TV episode.
/// </summary>
[JsonDerivedType(typeof(TvEpisodeBase))]
[JsonDerivedType(typeof(TvEpisode))]
[JsonDerivedType(typeof(TvGroupEpisode))]
[JsonDerivedType(typeof(TvEpisodeWithRating))]
public class TvEpisodeInfo
{
    /// <summary>
    /// Gets or sets the episode ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the season number.
    /// </summary>
    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; set; }

    /// <summary>
    /// Gets or sets the episode number.
    /// </summary>
    [JsonPropertyName("episode_number")]
    public long EpisodeNumber { get; set; }
}
