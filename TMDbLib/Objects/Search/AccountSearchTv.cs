using Newtonsoft.Json;

namespace TMDbLib.Objects.Search;

/// <summary>
/// TV show search result with the account's user rating.
/// </summary>
public class AccountSearchTv : SearchTv
{
    /// <summary>
    /// Gets or sets the user rating.
    /// </summary>
    [JsonProperty("rating")]
    public float Rating { get; set; }
}
