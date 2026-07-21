using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.People;

/// <summary>
/// Movie credits for a person.
/// </summary>
public class MovieCredits
{
    /// <summary>
    /// Gets or sets the cast roles.
    /// </summary>
    [JsonProperty("cast")]
    public List<CombinedCreditsCastMovie>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the crew jobs.
    /// </summary>
    [JsonProperty("crew")]
    public List<CombinedCreditsCrewMovie>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the person id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
