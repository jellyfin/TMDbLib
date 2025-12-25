using System;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using Xunit;

namespace TMDbLibTests.Helpers;

/// <summary>
/// General helper methods for tests.
/// </summary>
public static class TestHelpers
{
    /// <summary>
    /// Tests search pagination by verifying the first two pages of results.
    /// </summary>
    /// <typeparam name="T">The type of items in the search results.</typeparam>
    /// <param name="getter">A function that fetches a page of search results.</param>
    /// <returns>A task representing the asynchronous test operation.</returns>
    public static Task SearchPagesAsync<T>(Func<int, Task<SearchContainer<T>?>> getter)
    {
        return SearchPagesAsync<SearchContainer<T>, T>(getter);
    }

    /// <summary>
    /// Tests search pagination by verifying the first two pages of results with a custom container type.
    /// </summary>
    /// <typeparam name="TContainer">The container type for search results.</typeparam>
    /// <typeparam name="T">The type of items in the search results.</typeparam>
    /// <param name="getter">A function that fetches a page of search results.</param>
    /// <returns>A task representing the asynchronous test operation.</returns>
    public static async Task SearchPagesAsync<TContainer, T>(Func<int, Task<TContainer?>> getter) where TContainer : SearchContainer<T>
    {
        // Check page 1
        var results = await getter(1);

        Assert.NotNull(results);
        Assert.Equal(1, results.Page);
        Assert.NotNull(results.Results);
        Assert.NotEmpty(results.Results);
        Assert.True(results.Results.Count > 0);
        Assert.True(results.TotalResults > 0);
        Assert.True(results.TotalPages > 0);

        // Check page 2
        var results2 = await getter(2);

        Assert.NotNull(results2);
        Assert.Equal(2, results2.Page);

        // If page 1 has all results, page 2 must be empty - else not.
        if (results.Results.Count == results.TotalResults)
        {
            Assert.NotNull(results2.Results);
            Assert.Empty(results2.Results);
        }
        else
        {
            Assert.NotNull(results2.Results);
            Assert.NotEmpty(results2.Results);
        }
    }
}
