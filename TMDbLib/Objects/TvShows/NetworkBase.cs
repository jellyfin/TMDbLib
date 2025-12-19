using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Base class for TV network information.
/// </summary>
public class NetworkBase
{
    /// <summary>
    /// Gets or sets the network ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the network.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the origin country code of the network.
    /// </summary>
    [JsonProperty("origin_country")]
    public string OriginCountry { get; set; }
}
