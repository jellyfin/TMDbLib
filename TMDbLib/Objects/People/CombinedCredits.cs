using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents combined movie and TV credits for a person.
/// </summary>
public class CombinedCredits
{
    /// <summary>
    /// Gets or sets the list of combined cast roles (both movie and TV).
    /// </summary>
    [JsonProperty("cast", ItemConverterType = typeof(CombinedCreditsCastConverter))]
    public List<CombinedCreditsCastBase> Cast { get; set; }

    /// <summary>
    /// Gets or sets the list of combined crew jobs (both movie and TV).
    /// </summary>
    [JsonProperty("crew", ItemConverterType = typeof(CombinedCreditsCrewConverter))]
    public List<CombinedCreditsCrewBase> Crew { get; set; }

    /// <summary>
    /// Gets or sets the person ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
