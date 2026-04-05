using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a container for TV show translations.
/// </summary>
public class TranslationsContainerTv
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of translations.
    /// </summary>
    [JsonPropertyName("translations")]
    public List<Translation>? Translations { get; set; }
}
