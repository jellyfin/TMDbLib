using System;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a movie that a person is known for.
/// </summary>
public class KnownForMovie : KnownForBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KnownForMovie"/> class.
    /// </summary>
    public KnownForMovie()
    {
        MediaType = MediaType.Movie;
    }

    /// <summary>
    /// Gets or sets the original title of the movie.
    /// </summary>
    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

    /// <summary>
    /// Gets or sets the release date of the movie.
    /// </summary>
    [JsonPropertyName("release_date")]
    [JsonConverter(typeof(TmdbPartialDateConverter))]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the title of the movie.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is a video release.
    /// </summary>
    [JsonPropertyName("video")]
    public bool Video { get; set; }
}
