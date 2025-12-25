using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents an alternative title for a media item.
/// </summary>
public class AlternativeTitle
{
    /// <summary>
    /// Gets or sets a country code, e.g. US.
    /// </summary>
    [JsonProperty("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets the alternative title text.
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the type of title (e.g. working title, DVD title, modern title).
    /// </summary>
    [JsonProperty("type")]
    public string? Type { get; set; }
}
