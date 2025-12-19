using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a production company.
/// </summary>
public class ProductionCompany
{
    /// <summary>
    /// Gets or sets the TMDb ID of the production company.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the production company.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the logo path for the production company.
    /// </summary>
    [JsonProperty("logo_path")]
    public string LogoPath { get; set; }

    /// <summary>
    /// Gets or sets the origin country of the production company.
    /// </summary>
    [JsonProperty("origin_country")]
    public string OriginCountry { get; set; }
}
