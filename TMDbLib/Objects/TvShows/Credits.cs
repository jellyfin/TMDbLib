using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents credits information for a TV show or episode.
/// </summary>
public class Credits
{
    /// <summary>
    /// Gets or sets the list of cast members.
    /// </summary>
    [JsonPropertyName("cast")]
    public List<Cast>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the list of crew members.
    /// </summary>
    [JsonPropertyName("crew")]
    public List<Crew>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
