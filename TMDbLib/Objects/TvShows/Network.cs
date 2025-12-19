using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents detailed information about a TV network.
/// </summary>
public class Network : NetworkBase
{
    /// <summary>
    /// Gets or sets the headquarters location of the network.
    /// </summary>
    [JsonProperty("headquarters")]
    public string Headquarters { get; set; }

    /// <summary>
    /// Gets or sets the homepage URL of the network.
    /// </summary>
    [JsonProperty("homepage")]
    public string Homepage { get; set; }
}
