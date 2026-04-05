using System;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Reviews;

/// <summary>
/// Represents the base class for a review.
/// </summary>
[JsonDerivedType(typeof(Review))]
public class ReviewBase
{
    /// <summary>
    /// Gets or sets the author username.
    /// </summary>
    [JsonPropertyName("author")]
    public string? Author { get; set; }

    /// <summary>
    /// Gets or sets the detailed author information.
    /// </summary>
    [JsonPropertyName("author_details")]
    public AuthorDetails? AuthorDetails { get; set; }

    /// <summary>
    /// Gets or sets the review content.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    /// <summary>
    /// Gets or sets the review ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the URL to the full review.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
