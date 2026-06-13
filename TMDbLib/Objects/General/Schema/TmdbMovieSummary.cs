using System;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General.Schema;

/// <summary>
/// Movie-shaped media summary: adds the movie-only fields on top of <see cref="TmdbMediaSummary"/>.
/// </summary>
public class TmdbMovieSummary : TmdbMediaSummary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TmdbMovieSummary"/> class
    /// with <see cref="TmdbEntity.MediaType"/> set to <see cref="MediaType.Movie"/>.
    /// </summary>
    public TmdbMovieSummary()
    {
        MediaType = MediaType.Movie;
    }

    /// <summary>
    /// Gets or sets the original (untranslated) movie title.
    /// </summary>
    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonPropertyName("release_date")]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the movie title.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entry is a video release rather than a theatrical movie.
    /// </summary>
    [JsonPropertyName("video")]
    public bool Video { get; set; }
}
