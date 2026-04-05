using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Lists;

internal class ListCreateReply
{
    [JsonPropertyName("list_id")]
    public int? ListId { get; set; }

    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("status_message")]
    public string? StatusMessage { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
