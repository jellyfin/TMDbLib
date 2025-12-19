using Newtonsoft.Json;

namespace TMDbLib.Objects.Authentication;

/// <summary>
/// Session object that can be retrieved after the user has correctly authenticated himself on the TMDb site. (using the referal url from the token provided previously).
/// </summary>
public class UserSession
{
    /// <summary>
    /// Gets or sets the session ID.
    /// </summary>
    [JsonProperty("session_id")]
    public string SessionId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the session creation was successful.
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }
}
