using TMDbLib.Utilities;

namespace TMDbLib.Objects.Trending;

/// <summary>
/// Represents the time window for trending content.
/// </summary>
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
