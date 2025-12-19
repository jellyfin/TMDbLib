using Newtonsoft.Json;

namespace TMDbLib.Objects.Exceptions;

/// <summary>
/// Represents a status message returned by the TMDb API.
/// </summary>
public class TMDbStatusMessage
{
    /// <summary>
    /// Gets or sets the status code.
    /// </summary>
    [JsonProperty("status_code")]
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the status message text.
    /// </summary>
    [JsonProperty("status_message")]
    public string StatusMessage { get; set; }
}
