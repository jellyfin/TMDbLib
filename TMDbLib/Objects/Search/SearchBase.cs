using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Base class for all search results.
/// </summary>
public class SearchBase
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the media type.
    /// </summary>
    [JsonProperty("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonProperty("popularity")]
    public double Popularity { get; set; }
}
