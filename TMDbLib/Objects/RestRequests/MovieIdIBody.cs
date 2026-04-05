using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

public class MovieIdIBody : IBody
{
    public MovieIdIBody(int movieid)
    {
        MovieId = movieid;
    }

    [JsonPropertyName("media_id")]
    public int MovieId { get; set; }
}
