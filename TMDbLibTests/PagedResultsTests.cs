using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Utilities;
using Xunit;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for paging through the page-based endpoints.
/// </summary>
public class PagedResultsTests
{
    /// <summary>
    /// Tests that the items of every page are yielded, in page order, and that no request is made
    /// beyond the last page.
    /// </summary>
    [Fact]
    public async Task TestEnumerateAllAsyncYieldsEveryPage()
    {
        var requested = new List<int>();

        var items = await PagedResults.EnumerateAllAsync<int>(
            (page, _) =>
            {
                requested.Add(page);
                return Task.FromResult<IPagedResult<int>?>(new FakePage<int>(page, 3, 6, [page * 10, (page * 10) + 1]));
            },
            TestContext.Current.CancellationToken)
            .ToListAsync(TestContext.Current.CancellationToken);

        Assert.Equal([10, 11, 20, 21, 30, 31], items);
        Assert.Equal([1, 2, 3], requested);
    }

    /// <summary>
    /// Tests that a response which is not paged at all - TMDb leaves the paging fields off those,
    /// so they arrive as 0 - is handed out and ends the sequence.
    /// </summary>
    [Fact]
    public async Task TestEnumerateAllAsyncEndsOnUnpagedResponse()
    {
        var requests = 0;

        var items = await PagedResults.EnumerateAllAsync<int>(
            (_, _) =>
            {
                requests++;
                return Task.FromResult<IPagedResult<int>?>(new FakePage<int>(0, 0, 2, [1, 2]));
            },
            TestContext.Current.CancellationToken)
            .ToListAsync(TestContext.Current.CancellationToken);

        Assert.Equal([1, 2], items);
        Assert.Equal(1, requests);
    }

    /// <summary>
    /// Tests that a missing resource yields nothing rather than throwing.
    /// </summary>
    [Fact]
    public async Task TestEnumerateAllAsyncEmptyOnNullResponse()
    {
        var items = await PagedResults.EnumerateAllAsync<int>(
            (_, _) => Task.FromResult<IPagedResult<int>?>(null),
            TestContext.Current.CancellationToken)
            .ToListAsync(TestContext.Current.CancellationToken);

        Assert.Empty(items);
    }

    /// <summary>
    /// Tests that a page without items ends the sequence even when the response claims there is
    /// more to fetch, so a bad page count cannot spin forever.
    /// </summary>
    [Fact]
    public async Task TestEnumerateAllAsyncEndsOnEmptyPage()
    {
        var requests = 0;

        var items = await PagedResults.EnumerateAllAsync<int>(
            (page, _) =>
            {
                requests++;
                return Task.FromResult<IPagedResult<int>?>(new FakePage<int>(page, int.MaxValue, 1, page == 1 ? [1] : []));
            },
            TestContext.Current.CancellationToken)
            .ToListAsync(TestContext.Current.CancellationToken);

        Assert.Equal([1], items);
        Assert.Equal(2, requests);
    }

    /// <summary>
    /// Tests that enumeration stops at the highest page TMDb serves, however many pages a response
    /// claims to have.
    /// </summary>
    [Fact]
    public async Task TestEnumerateAllAsyncStopsAtMaxPage()
    {
        var requests = 0;

        var items = await PagedResults.EnumerateAllAsync<int>(
            (page, _) =>
            {
                requests++;
                return Task.FromResult<IPagedResult<int>?>(new FakePage<int>(page, int.MaxValue, int.MaxValue, [page]));
            },
            TestContext.Current.CancellationToken)
            .ToListAsync(TestContext.Current.CancellationToken);

        Assert.Equal(PagedResults.MaxPage, requests);
        Assert.Equal(PagedResults.MaxPage, items.Count);
    }

    /// <summary>
    /// Tests that the cancellation token reaches the page requests and stops the enumeration.
    /// </summary>
    [Fact]
    public async Task TestEnumerateAllAsyncPassesCancellationToken()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        var requests = 0;
        var enumerable = PagedResults.EnumerateAllAsync<int>(
            (page, token) =>
            {
                requests++;
                token.ThrowIfCancellationRequested();

                return Task.FromResult<IPagedResult<int>?>(new FakePage<int>(page, 2, 4, [page]));
            },
            cancellationTokenSource.Token);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await enumerable.ToListAsync(TestContext.Current.CancellationToken));
        Assert.Equal(1, requests);
    }

    private sealed record FakePage<T>(int Page, int TotalPages, int TotalResults, IReadOnlyList<T>? Items) : IPagedResult<T>;
}
