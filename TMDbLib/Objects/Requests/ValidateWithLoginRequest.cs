using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Requests;

internal class ValidateWithLoginRequest
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("request_token")]
    public string? RequestToken { get; set; }
}
