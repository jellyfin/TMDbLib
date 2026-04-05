using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a TV episode search result from an account with a user rating.
/// </summary>
public class AccountSearchTvEpisode : SearchTvEpisode
{
    /// <summary>
    /// Gets or sets the user rating for the TV episode.
    /// </summary>
    [JsonPropertyName("rating")]
    public double Rating { get; set; }
}
