using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// Credit entry for a person's work on a media item.
/// </summary>
public class Credit
{
    /// <summary>
    /// Gets or sets the credit type.
    /// </summary>
    [JsonPropertyName("credit_type")]
    public CreditType CreditType { get; set; }

    /// <summary>
    /// Gets or sets the crew department.
    /// </summary>
    [JsonPropertyName("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the credit id.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the crew job title.
    /// </summary>
    [JsonPropertyName("job")]
    public string? Job { get; set; }

    /// <summary>
    /// Gets or sets the media item.
    /// </summary>
    [JsonPropertyName("media")]
    public CreditMedia? Media { get; set; }

    /// <summary>
    /// Gets or sets the media type.
    /// </summary>
    [JsonPropertyName("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the person.
    /// </summary>
    [JsonPropertyName("person")]
    public CreditPerson? Person { get; set; }
}
