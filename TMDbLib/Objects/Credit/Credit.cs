using Newtonsoft.Json;
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
    [JsonProperty("credit_type")]
    public CreditType CreditType { get; set; }

    /// <summary>
    /// Gets or sets the crew department.
    /// </summary>
    [JsonProperty("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the credit id.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the crew job title.
    /// </summary>
    [JsonProperty("job")]
    public string? Job { get; set; }

    /// <summary>
    /// Gets or sets the media item.
    /// </summary>
    [JsonProperty("media")]
    public CreditMedia? Media { get; set; }

    /// <summary>
    /// Gets or sets the media type.
    /// </summary>
    [JsonProperty("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the person.
    /// </summary>
    [JsonProperty("person")]
    public CreditPerson? Person { get; set; }
}
