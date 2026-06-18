using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Search;

/// <summary>
/// TV episode search result with the account's user rating.
/// </summary>
public class AccountSearchTvEpisode : SearchTvEpisode
{
    /// <summary>
    /// Gets or sets the user rating.
    /// </summary>
    [JsonPropertyName("rating")]
    public double Rating { get; set; }
}
