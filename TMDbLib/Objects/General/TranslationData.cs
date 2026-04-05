using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents the translated data for a media item.
/// </summary>
public class TranslationData
{
    /// <summary>
    /// Gets or sets the translated name or title.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    // Private hack to ensure two properties (name, title) are deserialized into Name.
    // Tv Shows and Movies will use different names for their translation data.
    [JsonPropertyName("title")]
    private string Title
    {
        set => Name = value;
    }

    /// <summary>
    /// Gets or sets the translated overview or biography.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    // Private hack to ensure two properties (overview, biography) are deserialized into Overview.
    // Most of the entities have an overview, but people have a biography.
    [JsonPropertyName("biography")]
    private string Biography
    {
        set => Overview = value;
    }

    /// <summary>
    /// Gets or sets the translated homepage URL.
    /// </summary>
    [JsonPropertyName("homepage")]
    public string? HomePage { get; set; }

    /// <summary>
    /// Gets or sets the translated tagline.
    /// </summary>
    [JsonPropertyName("tagline")]
    public string? Tagline { get; set; }

    /// <summary>
    /// Gets or sets the runtime in minutes.
    /// </summary>
    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }
}
