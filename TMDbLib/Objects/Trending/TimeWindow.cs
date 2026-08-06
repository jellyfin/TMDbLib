using System.Text.Json.Serialization;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Trending;

/// <summary>
/// Represents the time window for trending content.
/// </summary>
[JsonConverter(typeof(TolerantEnumConverter<TimeWindow>))]
public enum TimeWindow
{
    /// <summary>
    /// Trending content for the current day.
    /// </summary>
    [EnumValue("day")]
    Day,

    /// <summary>
    /// Trending content for the current week.
    /// </summary>
    [EnumValue("week")]
    Week
}
