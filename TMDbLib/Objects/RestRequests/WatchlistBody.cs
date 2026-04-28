using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

/// <summary>
/// Represents a request body for updating account watchlist items.
/// </summary>
public class WatchlistBody : IBody
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WatchlistBody"/> class.
    /// </summary>
    /// <param name="mediaType">The media type to update.</param>
    /// <param name="mediaId">The media ID to update.</param>
    /// <param name="isOnWatchlist">Whether the item is on the watchlist.</param>
    public WatchlistBody(string mediaType, int mediaId, bool isOnWatchlist)
    {
        MediaType = mediaType;
        MediaId = mediaId;
        IsOnWatchlist = isOnWatchlist;
    }

    /// <summary>
    /// Gets or sets the media type to update.
    /// </summary>
    [JsonPropertyName("media_type")]
    public string MediaType { get; set; }

    /// <summary>
    /// Gets or sets the media ID to update.
    /// </summary>
    [JsonPropertyName("media_id")]
    public int MediaId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is on the watchlist.
    /// </summary>
    [JsonPropertyName("watchlist")]
    public bool IsOnWatchlist { get; set; }
}
