using Newtonsoft.Json;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Company search result.
/// </summary>
public class SearchCompany
{
    /// <summary>
    /// Gets or sets the company id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the logo image path.
    /// </summary>
    [JsonProperty("logo_path")]
    public string? LogoPath { get; set; }

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the origin country ISO code.
    /// </summary>
    [JsonProperty("origin_country")]
    public string? OriginCountry { get; set; }
}
