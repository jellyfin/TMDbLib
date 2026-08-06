using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Detailed TV network information.
/// </summary>
public class Network : NetworkWithLogo
{
    /// <summary>
    /// Gets or sets the headquarters location.
    /// </summary>
    [JsonPropertyName("headquarters")]
    public string? Headquarters { get; set; }

    /// <summary>
    /// Gets or sets the homepage URL.
    /// </summary>
    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }
}
