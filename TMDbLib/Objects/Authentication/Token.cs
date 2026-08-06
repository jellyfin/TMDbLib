using System;
using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Authentication;

/// <summary>
/// Request token used to create a user-authenticated session.
/// Tokens expire after 60 minutes and are consumed once a session is created.
/// </summary>
public class Token
{
    /// <summary>
    /// Gets or sets the authentication callback URL (populated by custom code).
    /// </summary>
    [JsonIgnore]
    public string? AuthenticationCallback { get; set; }

    /// <summary>
    /// Gets or sets the local date/time before which the token must be used.
    /// </summary>
    [JsonPropertyName("expires_at")]
    [JsonConverter(typeof(CustomDatetimeFormatConverter))]
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the request token.
    /// </summary>
    [JsonPropertyName("request_token")]
    public string? RequestToken { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the token request was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
