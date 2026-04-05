using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents movie credits for a person, including cast and crew roles.
/// </summary>
public class MovieCredits
{
    /// <summary>
    /// Gets or sets the list of cast roles.
    /// </summary>
    [JsonPropertyName("cast")]
    public List<MovieRole>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the list of crew jobs.
    /// </summary>
    [JsonPropertyName("crew")]
    public List<MovieJob>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the person ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
