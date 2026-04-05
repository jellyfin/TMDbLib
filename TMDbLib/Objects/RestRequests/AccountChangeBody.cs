using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

public class AccountChangeBody : IBody
{
    public AccountChangeBody(string mediaType, int mediaId, bool isFavorite)
    {
        MediaType = mediaType;
        MediaId = mediaId;
        IsFavorite = isFavorite;
    }

    [JsonPropertyName("media_type")]
    public string MediaType { get; set; }

    [JsonPropertyName("media_id")]
    public int MediaId { get; set; }

    [JsonPropertyName("favorite")]
    public bool IsFavorite { get; set; }
}
