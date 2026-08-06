using System.Text.Json.Serialization;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents sort order options.
/// </summary>
[JsonConverter(typeof(TolerantEnumConverter<SortOrder>))]
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
