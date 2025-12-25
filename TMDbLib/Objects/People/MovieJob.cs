using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents a crew job position for a person on a movie.
/// </summary>
public class MovieJob
{
    /// <summary>
    /// Gets or sets a value indicating whether the movie is adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonProperty("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the department name.
    /// </summary>
    [JsonProperty("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the movie ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the job title.
    /// </summary>
    [JsonProperty("job")]
    public string? Job { get; set; }

    /// <summary>
    /// Gets or sets the original title.
    /// </summary>
    [JsonProperty("original_title")]
    public string? OriginalTitle { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonProperty("release_date")]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the movie title.
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }
}
