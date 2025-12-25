using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents the TMDb API configuration.
/// </summary>
public class TMDbConfig
{
    /// <summary>
    /// Gets or sets the list of change keys.
    /// </summary>
    [JsonProperty("change_keys")]
    public List<string>? ChangeKeys { get; set; }

    /// <summary>
    /// Gets or sets the image configuration.
    /// </summary>
    [JsonProperty("images")]
    public ConfigImageTypes? Images { get; set; }
}
