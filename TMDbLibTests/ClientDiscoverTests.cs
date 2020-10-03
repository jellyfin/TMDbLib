using Xunit;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using System.Linq;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TMDbLibTests
{
    public class ClientDiscoverTests : TestBase
    {
        [Fact]
        public async Task TestDiscoverTvShowsNoParamsAsync()
        {
            await TestHelpers.SearchPagesAsync(i => TMDbClient.DiscoverTvShowsAsync().Query(i));

            SearchContainer<SearchTv> result = await TMDbClient.DiscoverTvShowsAsync().Query();

            Assert.NotNull(result);
            Assert.Equal(1, result.Page);
            Assert.NotNull(result.Results);
            Assert.True(result.Results.Any());
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
        public async Task TestDiscoverMoviesNoParamsAsync()
        {
            await TestHelpers.SearchPagesAsync(i => TMDbClient.DiscoverMoviesAsync().Query(i));

            SearchContainer<SearchMovie> result = await TMDbClient.DiscoverMoviesAsync().Query();

            Assert.NotNull(result);
            Assert.Equal(1, result.Page);
            Assert.NotNull(result.Results);
            Assert.True(result.Results.Any());
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
            DiscoverMovie query = TMDbClient.DiscoverMoviesAsync().WhereReleaseDateIsInRegion("BR").WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01));

            await TestHelpers.SearchPagesAsync(i => query.Query(i));
        }

        [Fact]
        public async Task TestDiscoverMoviesLanguageAsync()
        {
            DiscoverMovie query = TMDbClient.DiscoverMoviesAsync().WhereLanguageIs("da-DK").WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01));

            Assert.Equal("Skønheden og Udyret", (await query.Query(0)).Results[11].Title);

            await TestHelpers.SearchPagesAsync(i => query.Query(i));
        }

        [Theory]
        [InlineData("ko")]
        [InlineData("zh")]
        public async Task TestDiscoverMoviesOriginalLanguage(string language)
        {
            DiscoverMovie query = TMDbClient.DiscoverMoviesAsync().WhereOriginalLanguageIs(language);
            List<SearchMovie> results = (await query.Query(0)).Results;

            Assert.NotEmpty(results);
            Assert.All(results, item => Assert.Contains(language, item.OriginalLanguage));
        }
    }
}