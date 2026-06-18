using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Search;

/// <summary>
/// TV show search result with a user rating.
/// </summary>
public class SearchTvShowWithRating : SearchTv
{
    /// <summary>
    /// Gets or sets the user rating.
    /// </summary>
    [JsonPropertyName("rating")]
    public double Rating { get; set; }
}
