using System;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using Xunit;

namespace TMDbLibTests.Helpers
{
    public static class TestHelpers
    {
        public static Task SearchPagesAsync<T>(Func<int, Task<SearchContainer<T>>> getter)
        {
            return SearchPagesAsync<SearchContainer<T>, T>(getter);
        }

        public static async Task SearchPagesAsync<TContainer, T>(Func<int, Task<TContainer>> getter) where TContainer : SearchContainer<T>
        {
            // Check page 1
            TContainer results = await getter(1);
            
            Assert.Equal(1, results.Page);
            Assert.NotEmpty(results.Results);
            Assert.True(results.Results.Count > 0);
            Assert.True(results.TotalResults > 0);
            Assert.True(results.TotalPages > 0);

            // Check page 2
            TContainer results2 = await getter(2);
            
            Assert.Equal(2, results2.Page);

            // If page 1 has all results, page 2 must be empty - else not.
            if (results.Results.Count == results.TotalResults)
                Assert.Empty(results2.Results);
            else
                Assert.NotEmpty(results2.Results);
        }
    }
}
