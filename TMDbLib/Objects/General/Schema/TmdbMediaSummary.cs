using System.Collections.Generic;
using Newtonsoft.Json;
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
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the backdrop image path.
    /// </summary>
    [JsonProperty("backdrop_path")]
    public string? BackdropPath { get; set; }

    /// <summary>
    /// Gets or sets the genre IDs.
    /// </summary>
    [JsonProperty("genre_ids")]
    [JsonConverter(typeof(TmdbIntArrayAsObjectConverter)) /*#307*/]
    public List<int>? GenreIds { get; set; }

    /// <summary>
    /// Gets or sets the ISO 639-1 original language code.
    /// </summary>
    [JsonProperty("original_language")]
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Gets or sets the overview / synopsis text.
    /// </summary>
    [JsonProperty("overview")]
    public string? Overview { get; set; }

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
    /// Gets or sets the total vote count.
    /// </summary>
    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }
}
