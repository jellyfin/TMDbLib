using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents an alternative title for a media item.
/// </summary>
public class AlternativeTitle
{
    /// <summary>
    /// Gets or sets a country code, e.g. US.
    /// </summary>
    [JsonPropertyName("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets the alternative title text.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the type of title (e.g. working title, DVD title, modern title).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
