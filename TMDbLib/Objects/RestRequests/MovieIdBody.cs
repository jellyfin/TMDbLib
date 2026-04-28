using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

/// <summary>
/// Represents a request body containing a media ID.
/// </summary>
public class MovieIdBody : IBody
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MovieIdBody"/> class.
    /// </summary>
    /// <param name="movieid">The media ID.</param>
    public MovieIdBody(int movieid)
    {
        MovieId = movieid;
    }

    /// <summary>
    /// Gets or sets the media ID.
    /// </summary>
    [JsonPropertyName("media_id")]
    public int MovieId { get; set; }
}
