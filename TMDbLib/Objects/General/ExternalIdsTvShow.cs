using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents external IDs for a TV show.
/// </summary>
public class ExternalIdsTvShow : ExternalIds
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

    /// <summary>
    /// Gets or sets the Facebook ID.
    /// </summary>
    [JsonPropertyName("facebook_id")]
    public string? FacebookId { get; set; }

    /// <summary>
    /// Gets or sets the Twitter ID.
    /// </summary>
    [JsonPropertyName("twitter_id")]
    public string? TwitterId { get; set; }

    /// <summary>
    /// Gets or sets the Instagram ID.
    /// </summary>
    [JsonPropertyName("instagram_id")]
    public string? InstagramId { get; set; }
}
