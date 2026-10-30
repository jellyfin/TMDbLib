using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Requests;

internal class RequestTokenRequest
{
    [JsonPropertyName("request_token")]
    public string? RequestToken { get; set; }
}
