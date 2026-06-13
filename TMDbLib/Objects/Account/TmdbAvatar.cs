using Newtonsoft.Json;

namespace TMDbLib.Objects.Account;

/// <summary>
/// Represents the TMDb-hosted avatar metadata associated with an account.
/// </summary>
public class TmdbAvatar
{
    /// <summary>
    /// Gets or sets the relative path to the user's TMDb-hosted avatar image.
    /// </summary>
    [JsonProperty("avatar_path")]
    public string? AvatarPath { get; set; }
}
