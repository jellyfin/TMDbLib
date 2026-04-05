using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents aggregated credits information across multiple episodes or seasons.
/// </summary>
public class CreditsAggregate
{
    /// <summary>
    /// Gets or sets the list of aggregated cast members.
    /// </summary>
    [JsonPropertyName("cast")]
    public List<CastAggregate>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the list of aggregated crew members.
    /// </summary>
    [JsonPropertyName("crew")]
    public List<CrewAggregate>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
