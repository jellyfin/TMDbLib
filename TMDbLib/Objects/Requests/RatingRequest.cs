using System.Text.Json.Serialization;

namespace TMDbLib.Objects.Requests;

internal class RatingRequest
{
    [JsonPropertyName("value")]
    public double Value { get; set; }
}
