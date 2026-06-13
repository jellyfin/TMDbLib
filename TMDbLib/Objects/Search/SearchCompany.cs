using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Company search result.
/// </summary>
public class SearchCompany
{
    /// <summary>
    /// Gets or sets the company id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the logo image path.
    /// </summary>
    [JsonPropertyName("logo_path")]
    public string? LogoPath { get; set; }

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the origin country ISO code.
    /// </summary>
    [JsonPropertyName("origin_country")]
    public string? OriginCountry { get; set; }
}
