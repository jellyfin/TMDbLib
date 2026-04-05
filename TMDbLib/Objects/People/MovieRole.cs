using System;
using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents a movie role played by a person.
/// </summary>
public class MovieRole
{
    /// <summary>
    /// Gets or sets a value indicating whether the movie is adult content.
    /// </summary>
    [JsonPropertyName("adult")]
    public bool Adult { get; set; } = true;

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the genre IDs.
    /// </summary>
    [JsonPropertyName("genre_ids")]
    public int[]? GenreIds { get; set; }

    /// <summary>
    /// Gets or sets the movie ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the original language code.
    /// </summary>
    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the original title.
    /// </summary>
    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

    /// <summary>
    /// Gets or sets the movie overview.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    /// <summary>
    /// Gets or sets the poster image path.
    /// </summary>
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonPropertyName("release_date")]
    [JsonConverter(typeof(TmdbPartialDateConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the movie title.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the movie has video content.
    /// </summary>
    [JsonPropertyName("video")]
    public bool Video { get; set; } = true;

    /// <summary>
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the vote count.
    /// </summary>
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    /// <summary>
    /// Gets or sets the character name played in the movie.
    /// </summary>
    [JsonPropertyName("character")]
    public string? Character { get; set; }

    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the order in the cast.
    /// </summary>
    [JsonPropertyName("order")]
    public int Order { get; set; }
}
