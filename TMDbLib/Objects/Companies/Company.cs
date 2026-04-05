using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Objects.Companies;

/// <summary>
/// Represents a production company.
/// </summary>
public class Company
{
    /// <summary>
    /// Gets or sets the company description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the headquarters location.
    /// </summary>
    [JsonPropertyName("headquarters")]
    public string? Headquarters { get; set; }

    /// <summary>
    /// Gets or sets the company homepage URL.
    /// </summary>
    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }

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
    /// Gets or sets the movies associated with the company. This property is populated when the Movies method is requested.
    /// </summary>
    [JsonPropertyName("movies")]
    public SearchContainer<SearchMovie>? Movies { get; set; }

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the parent company information.
    /// </summary>
    [JsonPropertyName("parent_company")]
    public SearchCompany? ParentCompany { get; set; }

    /// <summary>
    /// Gets or sets the country of origin code.
    /// </summary>
    [JsonPropertyName("origin_country")]
    public string? OriginCountry { get; set; }
}
