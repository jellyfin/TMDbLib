using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents TV credits for a person, including cast and crew roles.
/// </summary>
public class TvCredits
{
    /// <summary>
    /// Gets or sets the list of TV cast roles.
    /// </summary>
    [JsonProperty("cast")]
    public List<TvRole> Cast { get; set; }

    /// <summary>
    /// Gets or sets the list of TV crew jobs.
    /// </summary>
    [JsonProperty("crew")]
    public List<TvJob> Crew { get; set; }

    /// <summary>
    /// Gets or sets the person ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
