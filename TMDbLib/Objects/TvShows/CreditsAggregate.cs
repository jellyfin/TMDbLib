using System.Collections.Generic;
using System.Text.Json.Serialization;
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
    [JsonPropertyName("cast")]
    public List<CastAggregate>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the aggregated crew members.
    /// </summary>
    [JsonPropertyName("crew")]
    public List<CrewAggregate>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the TMDb id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
