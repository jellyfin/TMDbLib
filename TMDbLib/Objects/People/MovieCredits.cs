using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.People;

/// <summary>
/// Movie credits for a person.
/// </summary>
public class MovieCredits
{
    /// <summary>
    /// Gets or sets the cast roles.
    /// </summary>
    [JsonPropertyName("cast")]
    public List<CombinedCreditsCastMovie>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the crew jobs.
    /// </summary>
    [JsonPropertyName("crew")]
    public List<CombinedCreditsCrewMovie>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the person id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
