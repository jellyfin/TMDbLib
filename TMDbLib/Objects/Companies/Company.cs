using Newtonsoft.Json;
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
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the headquarters location.
    /// </summary>
    [JsonProperty("headquarters")]
    public string? Headquarters { get; set; }

    /// <summary>
    /// Gets or sets the company homepage URL.
    /// </summary>
    [JsonProperty("homepage")]
    public string? Homepage { get; set; }

    /// <summary>
    /// Gets or sets the company ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the logo image path.
    /// </summary>
    [JsonProperty("logo_path")]
    public string? LogoPath { get; set; }

    /// <summary>
    /// Gets or sets the movies associated with the company. This property is populated when the Movies method is requested.
    /// </summary>
    [JsonProperty("movies")]
    public SearchContainer<SearchMovie>? Movies { get; set; }

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the parent company information.
    /// </summary>
    [JsonProperty("parent_company")]
    public SearchCompany? ParentCompany { get; set; }

    /// <summary>
    /// Gets or sets the country of origin code.
    /// </summary>
    [JsonProperty("origin_country")]
    public string? OriginCountry { get; set; }
}
