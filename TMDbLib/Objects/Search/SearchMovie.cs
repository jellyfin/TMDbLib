using System;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Represents a movie search result.
/// </summary>
public class SearchMovie : SearchMovieTvBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchMovie"/> class.
    /// </summary>
    public SearchMovie()
    {
        MediaType = MediaType.Movie;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the movie is adult content.
    /// </summary>
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the original title of the movie.
    /// </summary>
    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

    /// <summary>
    /// Gets or sets the release date of the movie.
    /// </summary>
    [JsonPropertyName("release_date")]
    [JsonConverter(typeof(IsoDateTimeConverterFactory))]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the title of the movie.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the movie is a video.
    /// </summary>
    [JsonPropertyName("video")]
    public bool Video { get; set; }
}
