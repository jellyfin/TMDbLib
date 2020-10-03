using System;
using System.Collections.Generic;
using System.Text;
using TMDbLib.Objects.Trending;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests
{
    public class ClientTrendingTests : TestBase
    {
        [Fact]
        public void TestTrendingMovies()
        {
            var movies = Config.Client.GetTrendingMoviesAsync(TimeWindow.Week).Result;
            Assert.True(movies.Results.Count > 0);
        }

        [Fact]
        public void TestTrendingTv()
        {
            var tv = Config.Client.GetTrendingTvAsync(TimeWindow.Week).Result;
            Assert.True(tv.Results.Count > 0);
        }

        [Fact]
        public void TestTrendingPeople()
        {
            var people = Config.Client.GetTrendingPeopleAsync(TimeWindow.Week).Result;
            Assert.True(people.Results.Count > 0);
        }
    }
}