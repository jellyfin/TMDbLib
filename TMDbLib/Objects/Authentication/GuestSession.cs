using System;
using Newtonsoft.Json;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Authentication;

/// <summary>
/// A guest session for rating movies/TV shows without a registered TMDb account.
/// Generate only one per user/device; sessions are discarded if unused within 24 hours.
/// </summary>
public class GuestSession
{
    /// <summary>
    /// Gets or sets the local date/time before which the session must first be used or it will expire.
    /// </summary>
    [JsonProperty("expires_at")]
    [JsonConverter(typeof(CustomDatetimeFormatConverter))]
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the guest session ID.
    /// </summary>
    [JsonProperty("guest_session_id")]
    public string? GuestSessionId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the guest session creation was successful.
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }
}
