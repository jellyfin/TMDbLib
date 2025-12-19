using Newtonsoft.Json;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a TV episode search result from an account with a user rating.
/// </summary>
public class AccountSearchTvEpisode : SearchTvEpisode
{
    /// <summary>
    /// Gets or sets the user rating for the TV episode.
    /// </summary>
    [JsonProperty("rating")]
    public double Rating { get; set; }
}
