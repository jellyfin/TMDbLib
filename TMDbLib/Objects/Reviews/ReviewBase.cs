using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.Reviews;

/// <summary>
/// Represents the base class for a review.
/// </summary>
public class ReviewBase
{
    /// <summary>
    /// Gets or sets the author username.
    /// </summary>
    [JsonProperty("author")]
    public string Author { get; set; }

    /// <summary>
    /// Gets or sets the detailed author information.
    /// </summary>
    [JsonProperty("author_details")]
    public AuthorDetails AuthorDetails { get; set; }

    /// <summary>
    /// Gets or sets the review content.
    /// </summary>
    [JsonProperty("content")]
    public string Content { get; set; }

    /// <summary>
    /// Gets or sets the review ID.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the URL to the full review.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date.
    /// </summary>
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
