using System.Text.Json.Serialization;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Account;

/// <summary>
/// Specifies the sorting options for account-related queries.
/// </summary>
[JsonConverter(typeof(TolerantEnumConverter<AccountSortBy>))]
public enum AccountSortBy
{
    /// <summary>
    /// No sorting specified.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// Sort by creation date.
    /// </summary>
    [EnumValue("created_at")]
    CreatedAt = 1,
}
