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
            await TestHelpers.SearchPagesAsync(i => Config.Client.DiscoverTvShowsAsync().Query(i));

            SearchContainer<SearchTv> result = await Config.Client.DiscoverTvShowsAsync().Query();

            Assert.NotNull(result);
            Assert.Equal(1, result.Page);
            Assert.NotNull(result.Results);
            Assert.True(result.Results.Any());
        }

        [Fact]
        public async Task TestDiscoverTvShowsAsync()
        {
            DiscoverTv query = Config.Client.DiscoverTvShowsAsync()
                    .WhereVoteCountIsAtLeast(100)
                    .WhereVoteAverageIsAtLeast(2);

            await TestHelpers.SearchPagesAsync(i => query.Query(i));
        }

        [Fact]
        public async Task TestDiscoverMoviesNoParamsAsync()
        {
            await TestHelpers.SearchPagesAsync(i => Config.Client.DiscoverMoviesAsync().Query(i));

            SearchContainer<SearchMovie> result = await Config.Client.DiscoverMoviesAsync().Query();

            Assert.NotNull(result);
            Assert.Equal(1, result.Page);
            Assert.NotNull(result.Results);
            Assert.True(result.Results.Any());
        }

        [Fact]
        public async Task TestDiscoverMoviesAsync()
        {
            DiscoverMovie query = Config.Client.DiscoverMoviesAsync()
                    .WhereVoteCountIsAtLeast(1000)
                    .WhereVoteAverageIsAtLeast(2);

            await TestHelpers.SearchPagesAsync(i => query.Query(i));
        }

        [Fact]
        public async Task TestDiscoverMoviesRegionAsync()
        {
            DiscoverMovie query = Config.Client.DiscoverMoviesAsync().WhereReleaseDateIsInRegion("BR").WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01));

            await TestHelpers.SearchPagesAsync(i => query.Query(i));
        }

        [Fact]
        public async Task TestDiscoverMoviesLanguageAsync()
        {
            DiscoverMovie query = Config.Client.DiscoverMoviesAsync().WhereLanguageIs("da-DK").WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01));

            Assert.Equal("Skønheden og Udyret", (await query.Query(0)).Results[11].Title);

            await TestHelpers.SearchPagesAsync(i => query.Query(i));
        }

        [Theory]
        [InlineData("ko")]
        [InlineData("zh")]
        public async Task TestDiscoverMoviesOriginalLanguage(string language)
        {
            DiscoverMovie query = Config.Client.DiscoverMoviesAsync().WhereOriginalLanguageIs(language);
            List<SearchMovie> results = (await query.Query(0)).Results;

            Assert.NotEmpty(results);
            Assert.All(results, item => Assert.Contains(language, item.OriginalLanguage));
        }
    }
}