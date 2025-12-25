using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a crew member with their job information.
/// </summary>
public class Crew : CrewBase
{
    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonProperty("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the job title.
    /// </summary>
    [JsonProperty("job")]
    public string? Job { get; set; }
}
