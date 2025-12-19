using Newtonsoft.Json;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Changes;

/// <summary>
/// Specifies the type of change action that occurred.
/// </summary>
[JsonConverter(typeof(EnumStringValueConverter))]
public enum ChangeAction
{
    /// <summary>
    /// Unknown or unspecified change action.
    /// </summary>
    Unknown,

    /// <summary>
    /// An item was added.
    /// </summary>
    [EnumValue("added")]
    Added = 1,

    /// <summary>
    /// An item was created.
    /// </summary>
    [EnumValue("created")]
    Created = 2,

    /// <summary>
    /// An item was updated.
    /// </summary>
    [EnumValue("updated")]
    Updated = 3,

    /// <summary>
    /// An item was deleted.
    /// </summary>
    [EnumValue("deleted")]
    Deleted = 4,

    /// <summary>
    /// An item was destroyed.
    /// </summary>
    [EnumValue("destroyed")]
    Destroyed = 5
}
