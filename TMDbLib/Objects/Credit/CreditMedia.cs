using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Credit;

/// <summary>
/// Media item in a credit context.
/// </summary>
public class CreditMedia
{
    /// <summary>
    /// Gets or sets a value indicating whether the media is adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonProperty("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the character name for acting credits.
    /// </summary>
    [JsonProperty("character")]
    public string? Character { get; set; }

    /// <summary>
    /// Gets or sets the episodes for this credit.
    /// </summary>
    [JsonProperty("episodes")]
    public List<CreditEpisode>? Episodes { get; set; }

    /// <summary>
    /// Gets or sets the first air date (TV).
    /// </summary>
    [JsonProperty("first_air_date")]
    public DateTime? FirstAirDate { get; set; }

    /// <summary>
    /// Gets or sets the genre ids.
    /// </summary>
    [JsonProperty("genre_ids")]
    public List<int>? GenreIds { get; set; }

    /// <summary>
    /// Gets or sets the TMDb id.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the media type.
    /// </summary>
    [JsonProperty("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the name (TV).
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the origin country ISO codes (TV).
    /// </summary>
    [JsonProperty("origin_country")]
    public List<string>? OriginCountry { get; set; }

    /// <summary>
    /// Gets or sets the original language code.
    /// </summary>
    [JsonProperty("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the original name (TV).
    /// </summary>
    [JsonProperty("original_name")]
    public string? OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the original title (movie).
    /// </summary>
    [JsonProperty("original_title")]
    public string? OriginalTitle { get; set; }

    /// <summary>
    /// Gets or sets the overview.
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
    /// Gets or sets the release date (movie).
    /// </summary>
    [JsonProperty("release_date")]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the seasons for this credit.
    /// </summary>
    [JsonProperty("seasons")]
    public List<CreditSeason>? Seasons { get; set; }

    /// <summary>
    /// Gets or sets the title (movie).
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the movie has video content.
    /// </summary>
    [JsonProperty("video")]
    public bool Video { get; set; }

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
