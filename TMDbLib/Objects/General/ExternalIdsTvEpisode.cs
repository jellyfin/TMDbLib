using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents external IDs for a TV episode.
/// </summary>
public class ExternalIdsTvEpisode : ExternalIds
{
    /// <summary>
    /// Gets or sets the IMDb ID.
    /// </summary>
    [JsonPropertyName("imdb_id")]
    public string? ImdbId { get; set; }

    /// <summary>
    /// Gets or sets the TVDb ID.
    /// </summary>
    [JsonPropertyName("tvdb_id")]
    public int? TvdbId { get; set; }
}
