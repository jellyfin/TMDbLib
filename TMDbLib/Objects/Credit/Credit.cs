using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// Represents a credit entry for a person's work on a media item.
/// </summary>
public class Credit
{
    /// <summary>
    /// Gets or sets the type of credit (cast or crew).
    /// </summary>
    [JsonProperty("credit_type")]
    public CreditType CreditType { get; set; }

    /// <summary>
    /// Gets or sets the department for crew credits.
    /// </summary>
    [JsonProperty("department")]
    public string Department { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for this credit.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the job title for crew credits.
    /// </summary>
    [JsonProperty("job")]
    public string Job { get; set; }

    /// <summary>
    /// Gets or sets the media item associated with this credit.
    /// </summary>
    [JsonProperty("media")]
    public CreditMedia Media { get; set; }

    /// <summary>
    /// Gets or sets the type of media (movie or TV show).
    /// </summary>
    [JsonProperty("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the person associated with this credit.
    /// </summary>
    [JsonProperty("person")]
    public CreditPerson Person { get; set; }
}
