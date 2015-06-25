using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using System.Linq;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientDiscoverTests
    {
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        [TestMethod]
        public void TestDiscoverTvShowsNoParams()
        {
            TestHelpers.SearchPages(i => _config.Client.DiscoverTvShows().Query(i).Result);

            SearchContainer<SearchTv> result = _config.Client.DiscoverTvShows().Query().Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Page);
            Assert.IsNotNull(result.Results);
            Assert.IsTrue(result.Results.Any());
        }

        [TestMethod]
        public void TestDiscoverTvShows()
        {
            DiscoverTv query = _config.Client.DiscoverTvShows()
                    .WhereVoteCountIsAtLeast(100)
                    .WhereVoteAverageIsAtLeast(2);

            TestHelpers.SearchPages(i => query.Query(i).Result);
        }

        [TestMethod]
        public void TestDiscoverMoviesNoParams()
        {
            TestHelpers.SearchPages(i => _config.Client.DiscoverMovies().Query(i).Result);

            SearchContainer<SearchMovie> result = _config.Client.DiscoverMovies().Query().Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Page);
            Assert.IsNotNull(result.Results);
            Assert.IsTrue(result.Results.Any());
        }

        [TestMethod]
        public void TestDiscoverMovies()
        {
            DiscoverMovie query = _config.Client.DiscoverMovies()
                    .WhereVoteCountIsAtLeast(1000)
                    .WhereVoteAverageIsAtLeast(2);

            TestHelpers.SearchPages(i => query.Query(i).Result);
        }
    }
}
