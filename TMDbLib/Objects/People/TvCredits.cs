using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.People;

/// <summary>
/// TV credits for a person.
/// </summary>
public class TvCredits
{
    /// <summary>
    /// Gets or sets the cast roles.
    /// </summary>
    [JsonProperty("cast")]
    public List<CombinedCreditsCastTv>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the crew jobs.
    /// </summary>
    [JsonProperty("crew")]
    public List<CombinedCreditsCrewTv>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the person id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
