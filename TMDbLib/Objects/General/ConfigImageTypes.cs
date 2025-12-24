using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents image configuration including available sizes and base URLs.
/// </summary>
public class ConfigImageTypes
{
    /// <summary>
    /// Gets or sets the list of available backdrop image sizes.
    /// </summary>
    [JsonProperty("backdrop_sizes")]
    public List<string>? BackdropSizes { get; set; }

    /// <summary>
    /// Gets or sets the base URL for image requests.
    /// </summary>
    [JsonProperty("base_url")]
    public string? BaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the list of available logo image sizes.
    /// </summary>
    [JsonProperty("logo_sizes")]
    public List<string>? LogoSizes { get; set; }

    /// <summary>
    /// Gets or sets the list of available poster image sizes.
    /// </summary>
    [JsonProperty("poster_sizes")]
    public List<string>? PosterSizes { get; set; }

    /// <summary>
    /// Gets or sets the list of available profile image sizes.
    /// </summary>
    [JsonProperty("profile_sizes")]
    public List<string>? ProfileSizes { get; set; }

    /// <summary>
    /// Gets or sets the secure base URL for image requests (HTTPS).
    /// </summary>
    [JsonProperty("secure_base_url")]
    public string? SecureBaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the list of available still image sizes.
    /// </summary>
    [JsonProperty("still_sizes")]
    public List<string>? StillSizes { get; set; }
}
