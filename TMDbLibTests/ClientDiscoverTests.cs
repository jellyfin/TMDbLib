using Xunit;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using System;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb discover functionality.
/// </summary>
public class ClientDiscoverTests : TestBase
{
    /// <summary>
    /// Tests that discovering TV shows without parameters returns results and supports pagination.
    /// </summary>
    [Fact]
    public async Task TestDiscoverTvShowsNoParamsAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.DiscoverTvShowsAsync().Query(i));
    }
    /// <summary>
    /// Tests that discovering movies without parameters returns results and supports pagination.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesNoParamsAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.DiscoverMoviesAsync().Query(i));
    }
    /// <summary>
    /// Tests that discovering TV shows with vote count and average filters returns filtered results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverTvShowsAsync()
    {
        DiscoverTv query = TMDbClient.DiscoverTvShowsAsync()
                .WhereVoteCountIsAtLeast(100)
                .WhereVoteAverageIsAtLeast(2);

        await TestHelpers.SearchPagesAsync(i => query.Query(i));
    }
    /// <summary>
    /// Tests that discovering movies with vote count and average filters returns filtered results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesAsync()
    {
        DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
                .WhereVoteCountIsAtLeast(1000)
                .WhereVoteAverageIsAtLeast(2);

        await TestHelpers.SearchPagesAsync(i => query.Query(i));
    }
    /// <summary>
    /// Tests that discovering movies with region and release date filters returns region-specific results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesRegionAsync()
    {
        DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
            .WhereReleaseDateIsInRegion("BR")
            .WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01));

        await TestHelpers.SearchPagesAsync(i => query.Query(i));
    }
    /// <summary>
    /// Tests that discovering movies with release type filters returns movies matching the specified release type.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesReleaseTypeAsync()
    {
        DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
            .WithAnyOfReleaseTypes(ReleaseDateType.Premiere);

        await TestHelpers.SearchPagesAsync(i => query.Query(i));
    }
    /// <summary>
    /// Tests that discovering movies with language filters returns localized titles while maintaining same movie IDs.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesLanguageAsync()
    {
        SearchContainer<SearchMovie> query = await TMDbClient.DiscoverMoviesAsync()
            .WhereOriginalLanguageIs("en-US")
            .WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01))
            .Query();

        SearchContainer<SearchMovie> queryDanish = await TMDbClient.DiscoverMoviesAsync()
            .WhereLanguageIs("da-DK")
            .WhereOriginalLanguageIs("en-US")
            .WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01))
            .Query();

        // Should be the same identities, but different titles
        Assert.Equal(query.TotalResults, queryDanish.TotalResults);

        for (int i = 0; i < query.Results.Count; i++)
        {
            SearchMovie a = query.Results[i];
            SearchMovie b = queryDanish.Results[i];

            Assert.Equal(a.Id, b.Id);
            Assert.NotEqual(a.Title, b.Title);
        }
    }
}
