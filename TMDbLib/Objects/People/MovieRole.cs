using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents a movie role played by a person.
/// </summary>
public class MovieRole
{
    /// <summary>
    /// Gets or sets a value indicating whether the movie is adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; } = true;

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonProperty("backdrop_path")]
    public string BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the genre IDs.
    /// </summary>
    [JsonProperty("genre_ids")]
    public int[] GenreIds { get; set; }

    /// <summary>
    /// Gets or sets the movie ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the original language code.
    /// </summary>
    [JsonProperty("original_language")]
    public string OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the original title.
    /// </summary>
    [JsonProperty("original_title")]
    public string OriginalTitle { get; set; }

    /// <summary>
    /// Gets or sets the movie overview.
    /// </summary>
    [JsonProperty("overview")]
    public string Overview { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonProperty("popularity")]
    public double Popularity { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonProperty("release_date", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the movie title.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the movie has video content.
    /// </summary>
    [JsonProperty("video")]
    public bool Video { get; set; } = true;

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

    /// <summary>
    /// Gets or sets the character name played in the movie.
    /// </summary>
    [JsonProperty("character")]
    public string Character { get; set; }

    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonProperty("credit_id")]
    public string CreditId { get; set; }

    /// <summary>
    /// Gets or sets the order in the cast.
    /// </summary>
    [JsonProperty("order")]
    public int Order { get; set; }
}
