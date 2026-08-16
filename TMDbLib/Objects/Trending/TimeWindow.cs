using TMDbLib.Utilities;

namespace TMDbLib.Objects.Trending;

/// <summary>
/// Represents the time window for trending content.
/// </summary>
[TolerantEnum]
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
