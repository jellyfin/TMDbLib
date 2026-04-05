using System.Text.Json.Serialization;
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
    [JsonPropertyName("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the media ID.
    /// </summary>
    [JsonPropertyName("media_id")]
    public int MediaId { get; set; }

    /// <summary>
    /// Gets or sets the media title.
    /// </summary>
    [JsonPropertyName("media_title")]
    public string? MediaTitle { get; set; }

    /// <summary>
    /// Gets or sets the media type.
    /// </summary>
    [JsonPropertyName("media_type")]
    public MediaType MediaType { get; set; }
}
