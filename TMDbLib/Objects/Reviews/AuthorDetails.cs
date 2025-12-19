using Newtonsoft.Json;

namespace TMDbLib.Objects.Reviews;

/// <summary>
/// Represents detailed information about a review author.
/// </summary>
public class AuthorDetails
{
    /// <summary>
    /// Gets or sets the author's display name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the author's username.
    /// </summary>
    [JsonProperty("username")]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the avatar image path.
    /// </summary>
    [JsonProperty("avatar_path")]
    public string AvatarPath { get; set; }

    /// <summary>
    /// Gets or sets the rating given by the author.
    /// </summary>
    [JsonProperty("rating")]
    public double? Rating { get; set; }
}
