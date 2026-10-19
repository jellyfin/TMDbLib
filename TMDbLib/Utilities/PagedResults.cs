using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;

namespace TMDbLib.Utilities;

/// <summary>
/// Turns the page-based TMDb endpoints into a single sequence of items.
/// </summary>
public static class PagedResults
{
    /// <summary>
    /// The highest page TMDb serves. Asking for more than this is an error on the paged endpoints,
    /// so enumeration stops here even when a response claims more pages.
    /// </summary>
    public const int MaxPage = 500;

    /// <summary>
    /// Pages through a TMDb endpoint, yielding every item of every page. One request is made per
    /// page, lazily, as the sequence is consumed - a long sequence is a lot of requests, so mind the
    /// rate limit when enumerating all of a large result set.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <param name="fetchPage">Fetches one page, given the 1-based page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The items of every page, in page order.</returns>
    public static async IAsyncEnumerable<T> EnumerateAllAsync<T>(
        Func<int, CancellationToken, Task<IPagedResult<T>?>> fetchPage,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fetchPage);

        for (var page = 1; page <= MaxPage; page++)
        {
            var result = await fetchPage(page, cancellationToken).ConfigureAwait(false);

            // No such resource, or nothing more to hand out.
            if (result?.Items is not { Count: > 0 } items)
            {
                yield break;
            }

            foreach (var item in items)
            {
                yield return item;
            }

            // TotalPages is 0 on a response that is not paged at all, so this also ends those after
            // their single page.
            if (page >= result.TotalPages)
            {
                yield break;
            }
        }
    }
}
