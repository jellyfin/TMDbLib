using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents a crew job position for a person on a TV show.
/// </summary>
public class TvJob
{
    /// <summary>
    /// Gets or sets a value indicating whether the TV show is adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonProperty("backdrop_path")]
    public string? BackdropPath { get; set; }

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
    /// Gets or sets the number of episodes.
    /// </summary>
    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }

    /// <summary>
    /// Gets or sets the first air date.
    /// </summary>
    [JsonProperty("first_air_date")]
    public DateTime? FirstAirDate { get; set; }

    /// <summary>
    /// Gets or sets the list of genre IDs.
    /// </summary>
    [JsonProperty("genre_ids")]
    public List<int>? GenreIds { get; set; }

    /// <summary>
    /// Gets or sets the TV show ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the job title.
    /// </summary>
    [JsonProperty("job")]
    public string? Job { get; set; }

    /// <summary>
    /// Gets or sets the TV show name.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets country ISO code ex. US.
    /// </summary>
    [JsonProperty("origin_country")]
    public List<string>? OriginCountry { get; set; }

    /// <summary>
    /// Gets or sets the original language code.
    /// </summary>
    [JsonProperty("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the original name.
    /// </summary>
    [JsonProperty("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the TV show overview.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonProperty("popularity")]
    public double Popularity { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the vote count.
    /// </summary>
    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }
}
