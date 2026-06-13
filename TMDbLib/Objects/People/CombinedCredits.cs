using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.People;

/// <summary>
/// Combined movie and TV credits for a person. Each list item is polymorphically a
/// <see cref="CombinedCreditsCastMovie"/>, <see cref="CombinedCreditsCastTv"/>,
/// <see cref="CombinedCreditsCrewMovie"/> or <see cref="CombinedCreditsCrewTv"/>.
/// </summary>
public class CombinedCredits
{
    /// <summary>
    /// Gets or sets the combined cast roles (both movie and TV).
    /// </summary>
    [JsonProperty("cast", ItemConverterType = typeof(CombinedCreditsCastConverter))]
    public List<TmdbMediaSummary>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the combined crew jobs (both movie and TV).
    /// </summary>
    [JsonProperty("crew", ItemConverterType = typeof(CombinedCreditsCrewConverter))]
    public List<TmdbMediaSummary>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the person id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
