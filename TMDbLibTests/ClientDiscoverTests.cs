using Xunit;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using System.Linq;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientDiscoverTests : TestBase
    {
        [Fact]
        public void TestDiscoverTvShowsNoParams()
        {
            TestHelpers.SearchPages(i => Config.Client.DiscoverTvShowsAsync().Query(i).Result);

            SearchContainer<SearchTv> result = Config.Client.DiscoverTvShowsAsync().Query().Result;

            Assert.NotNull(result);
            Assert.Equal(1, result.Page);
            Assert.NotNull(result.Results);
            Assert.True(result.Results.Any());
        }

        [Fact]
        public void TestDiscoverTvShows()
        {
            DiscoverTv query = Config.Client.DiscoverTvShowsAsync()
                    .WhereVoteCountIsAtLeast(100)
                    .WhereVoteAverageIsAtLeast(2);

            TestHelpers.SearchPages(i => query.Query(i).Result);
        }

        [Fact]
        public void TestDiscoverMoviesNoParams()
        {
            TestHelpers.SearchPages(i => Config.Client.DiscoverMoviesAsync().Query(i).Result);

            SearchContainer<SearchMovie> result = Config.Client.DiscoverMoviesAsync().Query().Result;

            Assert.NotNull(result);
            Assert.Equal(1, result.Page);
            Assert.NotNull(result.Results);
            Assert.True(result.Results.Any());
        }

        [Fact]
        public void TestDiscoverMovies()
        {
            DiscoverMovie query = Config.Client.DiscoverMoviesAsync()
                    .WhereVoteCountIsAtLeast(1000)
                    .WhereVoteAverageIsAtLeast(2);

            TestHelpers.SearchPages(i => query.Query(i).Result);
        }
    }
}
