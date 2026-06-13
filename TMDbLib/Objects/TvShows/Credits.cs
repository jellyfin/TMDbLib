using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Credits for a TV show or episode.
/// </summary>
public class Credits
{
    /// <summary>
    /// Gets or sets the cast members.
    /// </summary>
    [JsonPropertyName("cast")]
    public List<Cast>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the crew members.
    /// </summary>
    [JsonPropertyName("crew")]
    public List<Crew>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the TMDb id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
