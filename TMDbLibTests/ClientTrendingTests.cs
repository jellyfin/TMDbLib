using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests
{
    public class ClientTrendingTests : TestBase
    {
        [Fact]
        public async Task TestTrendingMoviesAsync()
        {
            SearchContainer<SearchMovie> movies = await TMDbClient.GetTrendingMoviesAsync(TimeWindow.Week);
            Assert.NotEmpty(movies.Results);
        }

        [Fact]
        public async Task TestTrendingTvAsync()
        {
            SearchContainer<SearchTv> tv = await TMDbClient.GetTrendingTvAsync(TimeWindow.Week);
            Assert.NotEmpty(tv.Results);
        }

        [Fact]
        public async Task TestTrendingPeopleAsync()
        {
            SearchContainer<SearchPerson> people = await TMDbClient.GetTrendingPeopleAsync(TimeWindow.Week);
            Assert.NotEmpty(people.Results);
        }

        [Fact]
        public async Task TestTrendingAllAsync()
        {
            SearchContainer<SearchBase> all = await TMDbClient.GetTrendingAllAsync(TimeWindow.Week);
            Assert.NotEmpty(all.Results);
        }
    }
}