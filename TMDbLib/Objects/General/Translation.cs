using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a translation for a media item.
/// </summary>
public class Translation
{
    /// <summary>
    /// Gets or sets the English name of the language.
    /// </summary>
    [JsonProperty("english_name")]
    public string? EnglishName { get; set; }

    /// <summary>
    /// Gets or sets a language code, e.g. en.
    /// </summary>
    [JsonProperty("iso_639_1")]
    public string? Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the country code.
    /// </summary>
    [JsonProperty("iso_3166_1")]
    public string? Iso_3166_1 { get; set; }

    /// <summary>
    /// Gets or sets the name of the language.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the translation data.
    /// </summary>
    [JsonProperty("data")]
    public TranslationData? Data { get; set; }
}
