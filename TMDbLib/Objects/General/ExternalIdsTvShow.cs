using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents external IDs for a TV show.
/// </summary>
public class ExternalIdsTvShow : ExternalIds
{
    /// <summary>
    /// Gets or sets the IMDb ID.
    /// </summary>
    [JsonProperty("imdb_id")]
    public string? ImdbId { get; set; }

    /// <summary>
    /// Gets or sets the TVDb ID.
    /// </summary>
    [JsonProperty("tvdb_id")]
    public string? TvdbId { get; set; }

    /// <summary>
    /// Gets or sets the Facebook ID.
    /// </summary>
    [JsonProperty("facebook_id")]
    public string? FacebookId { get; set; }

    /// <summary>
    /// Gets or sets the Twitter ID.
    /// </summary>
    [JsonProperty("twitter_id")]
    public string? TwitterId { get; set; }

    /// <summary>
    /// Gets or sets the Instagram ID.
    /// </summary>
    [JsonProperty("instagram_id")]
    public string? InstagramId { get; set; }
}
