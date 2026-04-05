using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

public sealed class RatingIBody : IBody
{
    public RatingIBody(double value)
    {
        Value = value;
    }

    [JsonPropertyName("value")]
    public double Value { get; set; }
}
