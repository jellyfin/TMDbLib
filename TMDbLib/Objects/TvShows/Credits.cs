using System.Collections.Generic;
using Newtonsoft.Json;
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
    [JsonProperty("cast")]
    public List<Cast>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the list of crew members.
    /// </summary>
    [JsonProperty("crew")]
    public List<Crew>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
