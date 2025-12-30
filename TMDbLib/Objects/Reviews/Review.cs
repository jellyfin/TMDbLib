using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Reviews;

/// <summary>
/// Represents a review with media information.
/// </summary>
public class Review : ReviewBase
{
    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonProperty("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the media ID.
    /// </summary>
    [JsonProperty("media_id")]
    public int MediaId { get; set; }

    /// <summary>
    /// Gets or sets the media title.
    /// </summary>
    [JsonProperty("media_title")]
    public string? MediaTitle { get; set; }

    /// <summary>
    /// Gets or sets the media type.
    /// </summary>
    [JsonProperty("media_type")]
    public MediaType MediaType { get; set; }
}
