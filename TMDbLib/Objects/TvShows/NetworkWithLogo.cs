using System.Text.Json.Serialization;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents a TV network with logo information.
/// </summary>
public class NetworkWithLogo : NetworkBase
{
    /// <summary>
    /// Gets or sets the logo image path.
    /// </summary>
    [JsonPropertyName("logo_path")]
    public string? LogoPath { get; set; }
}
