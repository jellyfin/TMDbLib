using System;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

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
    /// Gets or sets a value indicating whether the movie is adult content.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the original title of the movie.
    /// </summary>
    [JsonProperty("original_title")]
    public string OriginalTitle { get; set; }

    /// <summary>
    /// Gets or sets the release date of the movie.
    /// </summary>
    [JsonProperty("release_date")]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the title of the movie.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the movie is a video.
    /// </summary>
    [JsonProperty("video")]
    public bool Vide { get; set; }
}
