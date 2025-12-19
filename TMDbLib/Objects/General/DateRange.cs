using System;
using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents a date range with minimum and maximum dates.
/// </summary>
public class DateRange
{
    /// <summary>
    /// Gets or sets the maximum date in the range.
    /// </summary>
    [JsonProperty("maximum")]
    public DateTime Maximum { get; set; }

    /// <summary>
    /// Gets or sets the minimum date in the range.
    /// </summary>
    [JsonProperty("minimum")]
    public DateTime Minimum { get; set; }
}
