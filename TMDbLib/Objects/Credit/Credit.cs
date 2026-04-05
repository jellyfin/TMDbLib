using System.Text.Json.Serialization;
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
    [JsonPropertyName("credit_type")]
    public CreditType CreditType { get; set; }

    /// <summary>
    /// Gets or sets the department for crew credits.
    /// </summary>
    [JsonPropertyName("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for this credit.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the job title for crew credits.
    /// </summary>
    [JsonPropertyName("job")]
    public string? Job { get; set; }

    /// <summary>
    /// Gets or sets the media item associated with this credit.
    /// </summary>
    [JsonPropertyName("media")]
    public CreditMedia? Media { get; set; }

    /// <summary>
    /// Gets or sets the type of media (movie or TV show).
    /// </summary>
    [JsonPropertyName("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the person associated with this credit.
    /// </summary>
    [JsonPropertyName("person")]
    public CreditPerson? Person { get; set; }
}
