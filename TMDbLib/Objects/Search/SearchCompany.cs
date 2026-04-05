using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a company search result.
/// </summary>
public class SearchCompany
{
    /// <summary>
    /// Gets or sets the company ID.
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
}
