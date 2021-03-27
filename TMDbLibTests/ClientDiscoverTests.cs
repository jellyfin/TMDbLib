using Xunit;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using System;
using System.Threading.Tasks;

namespace TMDbLibTests
{
    public class ClientDiscoverTests : TestBase
    {
        [Fact]
        public async Task TestDiscoverTvShowsNoParamsAsync()
        {
            await TestHelpers.SearchPagesAsync(i => TMDbClient.DiscoverTvShowsAsync().Query(i));
        }

        [Fact]
        public async Task TestDiscoverMoviesNoParamsAsync()
        {
            await TestHelpers.SearchPagesAsync(i => TMDbClient.DiscoverMoviesAsync().Query(i));
        }

        [Fact]
        public async Task TestDiscoverTvShowsAsync()
        {
            DiscoverTv query = TMDbClient.DiscoverTvShowsAsync()
                    .WhereVoteCountIsAtLeast(100)
                    .WhereVoteAverageIsAtLeast(2);

            await TestHelpers.SearchPagesAsync(i => query.Query(i));
        }

        [Fact]
        public async Task TestDiscoverMoviesAsync()
        {
            DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
                    .WhereVoteCountIsAtLeast(1000)
                    .WhereVoteAverageIsAtLeast(2);

            await TestHelpers.SearchPagesAsync(i => query.Query(i));
        }

        [Fact]
        public async Task TestDiscoverMoviesRegionAsync()
        {
            DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
                .WhereReleaseDateIsInRegion("BR")
                .WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01));

            await TestHelpers.SearchPagesAsync(i => query.Query(i));
        }

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
}