using System;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.People;

/// <summary>
/// Represents a movie crew credit for a person.
/// </summary>
public class CombinedCreditsCrewMovie : CombinedCreditsCrewBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CombinedCreditsCrewMovie"/> class.
    /// </summary>
    public CombinedCreditsCrewMovie()
    {
        MediaType = MediaType.Movie;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the content is adult.
    /// </summary>
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    /// <summary>
    /// Gets or sets the original title.
    /// </summary>
    [JsonProperty("original_title")]
    public string OriginalTitle { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    [JsonProperty("release_date")]
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
    public bool Video { get; set; }
}
