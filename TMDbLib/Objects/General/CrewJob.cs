using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a crew member's job information.
/// </summary>
public class CrewJob
{
    /// <summary>
    /// Gets or sets the job title.
    /// </summary>
    [JsonPropertyName("job")]
    public string? Job { get; set; }

    /// <summary>
    /// Gets or sets the credit ID for this job.
    /// </summary>
    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the number of episodes associated with this job.
    /// </summary>
    [JsonPropertyName("episode_count")]
    public int EpisodeCount { get; set; }
}
