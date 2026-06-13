using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.General.Schema;

/// <summary>
/// Shared shape for any "movie or TV show summary" item TMDb returns.
/// Movie-only and TV-only fields live on <see cref="TmdbMovieSummary"/> / <see cref="TmdbTvSummary"/>.
/// </summary>
public class TmdbMediaSummary : TmdbEntity
{
    /// <summary>
    /// Gets or sets a value indicating whether the media is flagged as adult content.
    /// </summary>
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the genre IDs.
    /// </summary>
    [JsonPropertyName("genre_ids")]
    [JsonConverter(typeof(TmdbIntArrayAsObjectConverter)) /*#307*/]
    public List<int>? GenreIds { get; set; }

    /// <summary>
    /// Gets or sets the ISO 639-1 original language code.
    /// </summary>
    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the overview / synopsis text.
    /// </summary>
    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

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
    /// Gets or sets the total vote count.
    /// </summary>
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}
