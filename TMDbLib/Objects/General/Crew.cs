using Newtonsoft.Json;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Objects.General;

/// <summary>
/// Crew member with their job information.
/// </summary>
public class Crew : TmdbPersonSummary, ICrewCredit
{
    /// <summary>
    /// Gets or sets the credit identifier.
    /// </summary>
    [JsonProperty("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the crew department.
    /// </summary>
    [JsonProperty("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the specific job title.
    /// </summary>
    [JsonProperty("job")]
    public string? Job { get; set; }
}
