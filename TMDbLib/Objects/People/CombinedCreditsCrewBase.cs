using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.People;

/// <summary>
/// Base class for combined crew credits (movies and TV shows).
/// </summary>
[JsonDerivedType(typeof(CombinedCreditsCrewTv))]
[JsonDerivedType(typeof(CombinedCreditsCrewMovie))]
[JsonConverter(typeof(CombinedCreditsCrewConverter))]
public abstract class CombinedCreditsCrewBase
{
    /// <summary>
    /// Gets or sets the media type ("movie" or "tv").
    /// </summary>
    [JsonPropertyName("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the ID of the movie or TV show.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the department.
    /// </summary>
    [JsonPropertyName("department")]
    public string? Department { get; set; }

    /// <summary>
    /// Gets or sets the job title.
    /// </summary>
    [JsonPropertyName("job")]
    public string? Job { get; set; }

    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonPropertyName("credit_id")]
    public string? CreditId { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the genre IDs.
    /// </summary>
    [JsonPropertyName("genre_ids")]
    [JsonConverter(typeof(TmdbIntArrayAsObjectConverter))]
    public List<int>? GenreIds { get; set; }

    /// <summary>
    /// Gets or sets the original language code.
    /// </summary>
    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the overview.
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
    /// Gets or sets the average vote score.
    /// </summary>
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    /// <summary>
    /// Gets or sets the vote count.
    /// </summary>
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}
