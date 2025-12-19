using Newtonsoft.Json;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a TV show search result with a user rating.
/// </summary>
public class SearchTvShowWithRating : SearchTv
{
    /// <summary>
    /// Gets or sets the user rating for the TV show.
    /// </summary>
    [JsonProperty("rating")]
    public double Rating { get; set; }
}
