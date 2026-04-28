using System.Text.Json.Serialization;

namespace TMDbLib.Objects.RestRequests;

/// <summary>
/// Represents a request body containing a rating value.
/// </summary>
public sealed class RatingBody : IBody
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RatingBody"/> class.
    /// </summary>
    /// <param name="value">The rating value.</param>
    public RatingBody(double value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets or sets the rating value.
    /// </summary>
    [JsonPropertyName("value")]
    public double Value { get; set; }
}
