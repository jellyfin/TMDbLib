using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Base class for TV network information.
/// </summary>
public class NetworkBase
{
    /// <summary>
    /// Gets or sets the network id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the network name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the origin country code.
    /// </summary>
    [JsonPropertyName("origin_country")]
    public string? OriginCountry { get; set; }
}
