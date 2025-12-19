using Newtonsoft.Json;

namespace TMDbLib.Objects.Languages;

/// <summary>
/// Represents a language.
/// </summary>
public class Language
{
    /// <summary>
    /// Gets or sets the ISO 639-1 language code.
    /// </summary>
    [JsonProperty("iso_639_1")]
    public string Iso_639_1 { get; set; }

    /// <summary>
    /// Gets or sets the English name of the language.
    /// </summary>
    [JsonProperty("english_name")]
    public string EnglishName { get; set; }

    /// <summary>
    /// Gets or sets the native name of the language.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
}
