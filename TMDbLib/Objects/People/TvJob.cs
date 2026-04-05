using System;
using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents a crew job position for a person on a TV show.
/// </summary>
public class TvJob
{
    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the department name.
    /// </summary>
    [JsonPropertyName("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the number of episodes.
    /// </summary>
    [JsonPropertyName("episode_count")]
    public int EpisodeCount { get; set; }

    /// <summary>
    /// Gets or sets the first air date.
    /// </summary>
    [JsonPropertyName("first_air_date")]
    [JsonConverter(typeof(TmdbPartialDateConverter))]

    public DateTime? FirstAirDate { get; set; }

    /// <summary>
    /// Gets or sets the TV show ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the job title.
    /// </summary>
    [JsonPropertyName("job")]
    public string? Job { get; set; }

    /// <summary>
    /// Gets or sets the TV show name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the original name.
    /// </summary>
    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }
}
