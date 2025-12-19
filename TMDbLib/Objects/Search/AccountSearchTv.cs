using Newtonsoft.Json;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a TV show search result from an account with a user rating.
/// </summary>
public class AccountSearchTv : SearchTv
{
    /// <summary>
    /// Gets or sets the user rating for the TV show.
    /// </summary>
    [JsonProperty("rating")]
    public float Rating { get; set; }
}
