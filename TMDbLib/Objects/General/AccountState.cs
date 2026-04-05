using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents the account state for a specific media item.
/// </summary>
public class AccountState
{
    /// <summary>
    /// Gets or sets a value indicating whether the related movie is favorited for the current user session.
    /// </summary>
    [JsonPropertyName("favorite")]
    public bool Favorite { get; set; }

    /// <summary>
    /// Gets or sets the TMDb id for the related movie.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user's rating for the media item.
    /// </summary>
    [JsonPropertyName("rated")]
    [JsonConverter(typeof(TmdbRatingConverterFactory))]
    public double? Rating { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the related movie is on the current user's watchlist.
    /// </summary>
    [JsonPropertyName("watchlist")]
    public bool Watchlist { get; set; }
}
