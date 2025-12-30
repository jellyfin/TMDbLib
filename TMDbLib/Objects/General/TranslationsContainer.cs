using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a container for translations.
/// </summary>
public class TranslationsContainer
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of translations.
    /// </summary>
    [JsonProperty("translations")]
    public List<Translation>? Translations { get; set; }
}
