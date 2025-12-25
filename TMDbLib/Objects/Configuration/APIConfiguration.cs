using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Configuration;

/// <summary>
/// Represents the TMDb API configuration including image settings and change keys.
/// </summary>
public class APIConfiguration
{
    /// <summary>
    /// Gets or sets the image configuration settings.
    /// </summary>
    [JsonProperty("images")]
    public APIConfigurationImages? Images { get; set; }

    /// <summary>
    /// Gets or sets the list of available change keys.
    /// </summary>
    [JsonProperty("change_keys")]
    public List<string>? ChangeKeys { get; set; }
}
