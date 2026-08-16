using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Requests;

internal class FavoriteRequest
{
    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }

    [JsonPropertyName("media_id")]
    public int MediaId { get; set; }

    [JsonPropertyName("favorite")]
    public bool Favorite { get; set; }
}
