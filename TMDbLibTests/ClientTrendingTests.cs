using TMDbLib.Objects.Trending;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests
{
    public class ClientTrendingTests : TestBase
    {
        public ClientTrendingTests(TestConfig testConfig) : base(testConfig)
        {
        }

        [Fact]
        public void TestTrendingMovies()
        {
            IgnoreMissingJson("results[array] / media_type");

            var movies = Config.Client.GetTrendingMoviesAsync(TimeWindow.Week).Result;
            Assert.True(movies.Results.Count > 0);
        }

        [Fact]
        public void TestTrendingTv()
        {
            IgnoreMissingJson("results[array] / media_type");

            var tv = Config.Client.GetTrendingTvAsync(TimeWindow.Week).Result;
            Assert.True(tv.Results.Count > 0);
        }

        [Fact]
        public void TestTrendingPeople()
        {
            IgnoreMissingCSharp("results[array].gender / gender", "results[array].known_for_department / known_for_department");
            IgnoreMissingJson(" / popularity", "results[array] / media_type");

            var people = Config.Client.GetTrendingPeopleAsync(TimeWindow.Week).Result;
            Assert.True(people.Results.Count > 0);
        }
    }
}