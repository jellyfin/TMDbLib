using TMDbLib.Utilities;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents sort order options.
/// </summary>
public enum SortOrder
{
    /// <summary>
    /// Undefined sort order.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// Ascending sort order.
    /// </summary>
    [EnumValue("asc")]
    Ascending = 1,

    /// <summary>
    /// Descending sort order.
    /// </summary>
    [EnumValue("desc")]
    Descending = 2
}
