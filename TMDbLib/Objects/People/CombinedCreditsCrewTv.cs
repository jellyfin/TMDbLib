using Newtonsoft.Json;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Objects.People;

/// <summary>
/// TV crew credit for a person.
/// </summary>
public class CombinedCreditsCrewTv : TmdbTvSummary, ICrewCredit, ITvCreditExtras
{
    /// <summary>
    /// Gets or sets the credit id.
    /// </summary>
    [JsonProperty("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the crew department.
    /// </summary>
    [JsonProperty("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the number of episodes the person worked on.
    /// </summary>
    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }

    /// <summary>
    /// Gets or sets the job title.
    /// </summary>
    [JsonProperty("job")]
    public string? Job { get; set; }
}
