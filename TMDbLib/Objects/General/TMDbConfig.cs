using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents the TMDb API configuration.
/// </summary>
public class TMDbConfig
{
    /// <summary>
    /// Gets or sets the list of change keys.
    /// </summary>
    [JsonPropertyName("change_keys")]
    public List<string>? ChangeKeys { get; set; }

    /// <summary>
    /// Gets or sets the image configuration.
    /// </summary>
    [JsonPropertyName("images")]
    public ConfigImageTypes? Images { get; set; }
}
