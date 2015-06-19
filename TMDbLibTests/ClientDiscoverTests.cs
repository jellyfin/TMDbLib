using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using System.Linq;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
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
            TestHelpers.SearchPages(i => _config.Client.DiscoverTvShows(i));

            SearchContainer<SearchTv> result = _config.Client.DiscoverTvShows();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Page);
            Assert.IsNotNull(result.Results);
            Assert.IsTrue(result.Results.Any());
        }

        [TestMethod]
        public void TestDiscoverMoviesNoParams()
        {
            TestHelpers.SearchPages(i => _config.Client.DiscoverMovies(i));

            SearchContainer<SearchMovie> result = _config.Client.DiscoverMovies();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Page);
            Assert.IsNotNull(result.Results);
            Assert.IsTrue(result.Results.Any());
        }
    }
}
