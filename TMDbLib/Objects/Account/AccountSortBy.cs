using TMDbLib.Utilities;

namespace TMDbLib.Objects.Account;

/// <summary>
/// Specifies the sorting options for account-related queries.
/// </summary>
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
