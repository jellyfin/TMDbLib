using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents a TV role played by a person.
/// </summary>
public class TvRole
{
    /// <summary>
    /// Gets or sets the character name.
    /// </summary>
    [JsonProperty("character")]
    public string Character { get; set; }

    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonProperty("credit_id")]
    public string CreditId { get; set; }

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
    /// Gets or sets the TV show ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the TV show name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the original name.
    /// </summary>
    [JsonProperty("original_name")]
    public string OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }
}
