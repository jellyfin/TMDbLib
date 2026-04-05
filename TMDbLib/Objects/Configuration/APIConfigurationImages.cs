using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Configuration;

/// <summary>
/// Represents the image configuration settings for TMDb API, including base URLs and available sizes.
/// </summary>
public class APIConfigurationImages
{
    /// <summary>
    /// Gets or sets the base URL for images.
    /// </summary>
    [JsonPropertyName("base_url")]
    public string? BaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the secure base URL (HTTPS) for images.
    /// </summary>
    [JsonPropertyName("secure_base_url")]
    public string? SecureBaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the list of available backdrop image sizes.
    /// </summary>
    [JsonPropertyName("backdrop_sizes")]
    public List<string>? BackdropSizes { get; set; }

    /// <summary>
    /// Gets or sets the list of available logo image sizes.
    /// </summary>
    [JsonPropertyName("logo_sizes")]
    public List<string>? LogoSizes { get; set; }

    /// <summary>
    /// Gets or sets the list of available poster image sizes.
    /// </summary>
    [JsonPropertyName("poster_sizes")]
    public List<string>? PosterSizes { get; set; }

    /// <summary>
    /// Gets or sets the list of available profile image sizes.
    /// </summary>
    [JsonPropertyName("profile_sizes")]
    public List<string>? ProfileSizes { get; set; }

    /// <summary>
    /// Gets or sets the list of available still image sizes.
    /// </summary>
    [JsonPropertyName("still_sizes")]
    public List<string>? StillSizes { get; set; }
}
