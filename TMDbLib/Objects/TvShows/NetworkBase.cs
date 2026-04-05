using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Base class for TV network information.
/// </summary>
[JsonDerivedType(typeof(Network))]
[JsonDerivedType(typeof(NetworkWithLogo))]
public class NetworkBase
{
    /// <summary>
    /// Gets or sets the network ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the network.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the origin country code of the network.
    /// </summary>
    [JsonPropertyName("origin_country")]
    public string? OriginCountry { get; set; }
}
