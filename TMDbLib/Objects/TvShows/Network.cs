using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Detailed TV network information.
/// </summary>
public class Network : NetworkWithLogo
{
    /// <summary>
    /// Gets or sets the headquarters location.
    /// </summary>
    [JsonProperty("headquarters")]
    public string? Headquarters { get; set; }

    /// <summary>
    /// Gets or sets the homepage URL.
    /// </summary>
    [JsonProperty("homepage")]
    public string? Homepage { get; set; }
}
