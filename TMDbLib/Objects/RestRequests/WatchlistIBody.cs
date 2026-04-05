using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

public class WatchlistIBody : IBody
{
    public WatchlistIBody(string mediaType, int mediaId, bool isOnWatchlist)
    {
        MediaType = mediaType;
        MediaId = mediaId;
        IsOnWatchlist = isOnWatchlist;
    }

    [JsonPropertyName("media_type")]
    public string MediaType { get; set; }

    [JsonPropertyName("media_id")]
    public int MediaId { get; set; }

    [JsonPropertyName("watchlist")]
    public bool IsOnWatchlist { get; set; }
}
