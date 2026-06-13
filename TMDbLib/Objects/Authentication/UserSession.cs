using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Authentication;

/// <summary>
/// User session retrieved after the user authenticates on TMDb via the token's referral URL.
/// </summary>
public class UserSession
{
    /// <summary>
    /// Gets or sets the session ID.
    /// </summary>
    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the session creation was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
