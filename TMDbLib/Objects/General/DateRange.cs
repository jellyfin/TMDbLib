using System;
using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a date range with minimum and maximum dates.
/// </summary>
public class DateRange
{
    /// <summary>
    /// Gets or sets the maximum date in the range.
    /// </summary>
    [JsonPropertyName("maximum")]
    public DateTime Maximum { get; set; }

    /// <summary>
    /// Gets or sets the minimum date in the range.
    /// </summary>
    [JsonPropertyName("minimum")]
    public DateTime Minimum { get; set; }
}
