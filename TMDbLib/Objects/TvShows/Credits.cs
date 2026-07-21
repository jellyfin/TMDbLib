using System.Collections.Generic;
using Newtonsoft.Json;
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
    [JsonProperty("cast")]
    public List<Cast>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the crew members.
    /// </summary>
    [JsonProperty("crew")]
    public List<Crew>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the TMDb id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
