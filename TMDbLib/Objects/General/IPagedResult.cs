using System.Collections.Generic;

namespace TMDbLib.Objects.General;

/// <summary>
/// A response that TMDb serves one page at a time. Implemented by every paged response so a caller
/// can page through all of it without knowing which endpoint produced it.
/// </summary>
/// <typeparam name="T">The type of the items on the page.</typeparam>
public interface IPagedResult<out T>
{
    /// <summary>
    /// Gets the page this response holds.
    /// </summary>
    int Page { get; }

    /// <summary>
    /// Gets the total number of pages available.
    /// </summary>
    int TotalPages { get; }

    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    int TotalResults { get; }

    /// <summary>
    /// Gets the items on this page.
    /// </summary>
    IReadOnlyList<T>? Items { get; }
}
