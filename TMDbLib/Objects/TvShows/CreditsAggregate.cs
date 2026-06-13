using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Aggregated credits across episodes or seasons.
/// </summary>
public class CreditsAggregate
{
    /// <summary>
    /// Gets or sets the aggregated cast members.
    /// </summary>
    [JsonProperty("cast")]
    public List<CastAggregate>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the aggregated crew members.
    /// </summary>
    [JsonProperty("crew")]
    public List<CrewAggregate>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the TMDb id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
