using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Requests;

internal class AccessTokenRequest
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
}
