using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

internal class PostReply
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("status_message")]
    public string? StatusMessage { get; set; }
}
