using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Requests;

internal class MediaIdRequest
{
    [JsonPropertyName("media_id")]
    public int MediaId { get; set; }
}
