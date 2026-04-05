using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents TV credits for a person, including cast and crew roles.
/// </summary>
public class TvCredits
{
    /// <summary>
    /// Gets or sets the list of TV cast roles.
    /// </summary>
    [JsonPropertyName("cast")]
    public List<TvRole>? Cast { get; set; }

    /// <summary>
    /// Gets or sets the list of TV crew jobs.
    /// </summary>
    [JsonPropertyName("crew")]
    public List<TvJob>? Crew { get; set; }

    /// <summary>
    /// Gets or sets the person ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
