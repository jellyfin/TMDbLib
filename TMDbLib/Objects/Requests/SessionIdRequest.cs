using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Requests;

internal class SessionIdRequest
{
    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }
}
