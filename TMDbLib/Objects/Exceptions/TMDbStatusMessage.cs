using System;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Exceptions;

/// <summary>
/// Represents a status message returned by the TMDb API.
/// </summary>
public class TMDbStatusMessage
{
    /// <summary>
    /// Gets or sets the raw status code as returned by TMDb.
    /// </summary>
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the status message text.
    /// </summary>
    [JsonPropertyName("status_message")]
    public string? StatusMessage { get; set; }

    /// <summary>
    /// Gets the TMDb status code mapped to a known <see cref="TMDbStatusCode"/> value.
    /// Returns <see cref="TMDbStatusCode.Unknown"/> if the code is outside the documented range.
    /// </summary>
    [JsonIgnore]
    public TMDbStatusCode Code =>
        Enum.IsDefined(typeof(TMDbStatusCode), StatusCode) ? (TMDbStatusCode)StatusCode : TMDbStatusCode.Unknown;
}
