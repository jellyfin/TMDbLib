using System;
using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Authentication;

/// <summary>
/// A request token is required in order to request a user authenticated session id.
/// Request tokens will expire after 60 minutes.
/// As soon as a valid session id has been created the token will be useless.
/// </summary>
public class Token
{
    /// <summary>
    /// Gets or sets the authentication callback URL. This field is populated by custom code.
    /// </summary>
    // This field is populated by custom code
    [JsonIgnore]
    public string? AuthenticationCallback { get; set; }

    /// <summary>
    /// Gets or sets the date / time before which the token must be used, else it will expire. Time is expressed as local time.
    /// </summary>
    [JsonPropertyName("expires_at")]
    [JsonConverter(typeof(TmdbUtcTimeConverterFactory))]
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
