using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

/// <summary>
/// Represents a request body for updating account favorites.
/// </summary>
public class AccountChangeBody : IBody
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountChangeBody"/> class.
    /// </summary>
    /// <param name="mediaType">The media type to update.</param>
    /// <param name="mediaId">The media ID to update.</param>
    /// <param name="isFavorite">Whether the item is marked as favorite.</param>
    public AccountChangeBody(string mediaType, int mediaId, bool isFavorite)
    {
        MediaType = mediaType;
        MediaId = mediaId;
        IsFavorite = isFavorite;
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
    /// Gets or sets a value indicating whether the item is marked as favorite.
    /// </summary>
    [JsonPropertyName("favorite")]
    public bool IsFavorite { get; set; }
}
