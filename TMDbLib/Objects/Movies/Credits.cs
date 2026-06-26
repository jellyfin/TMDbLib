using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Movies;

/// <summary>
/// Cast and crew credits for a movie.
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
    /// Gets or sets the movie id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
