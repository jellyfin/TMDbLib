using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using System.Linq;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientSearchTests
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
        public void TestSearchMovie()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchMovie("007", i).Result);

            // Search pr. Year
            // 1962: First James Bond movie, "Dr. No"
            SearchContainer<SearchMovie> result = _config.Client.SearchMovie("007", year: 1962).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Page);
            Assert.AreEqual(1, result.Results.Count);
            Assert.AreEqual(1, result.TotalResults);
        }

        [TestMethod]
        public void TestSearchCollection()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchCollection("007", i).Result);

            SearchContainer<SearchResultCollection> result = _config.Client.SearchCollection("James Bond").Result;

            Assert.IsTrue(result.Results.Any(s => s.Id == 645));
            Assert.IsTrue(result.Results.Any(s => s.Name == "James Bond Collection"));
        }

        [TestMethod]
        public void TestSearchPerson()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchPerson("Willis", i).Result);

            SearchContainer<SearchPerson> result = _config.Client.SearchPerson("Willis").Result;

            Assert.IsTrue(result.Results.Any(s => s.Id == 62));
            Assert.IsTrue(result.Results.Any(s => s.Name == "Bruce Willis"));
        }

        [TestMethod]
        public void TestSearchList()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchList("to watch", i).Result);

            SearchContainer<SearchList> result = _config.Client.SearchList("2013").Result;

            Assert.IsTrue(result.Results.Any(s => s.Id == "50cbe90b19c2956de8047b4f"));
            Assert.IsTrue(result.Results.Any(s => s.Name == "Sci-Fi films to see in 2013"));
        }

        [TestMethod]
        public void TestSearchCompany()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchCompany("20th", i).Result);

            SearchContainer<SearchCompany> result = _config.Client.SearchCompany("20th").Result;

            Assert.IsTrue(result.Results.Any(s => s.Id == 25));
            Assert.IsTrue(result.Results.Any(s => s.Name == "20th Century Fox"));
        }

        [TestMethod]
        public void TestSearchKeyword()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchKeyword("plot", i).Result);

            SearchContainer<SearchKeyword> result = _config.Client.SearchKeyword("plot").Result;

            Assert.IsTrue(result.Results.Any(s => s.Id == 11422));
            Assert.IsTrue(result.Results.Any(s => s.Name == "plot twist"));
        }

        [TestMethod]
        public void TestSearchTvShow()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchTvShow("Breaking Bad", i).Result);

            SearchContainer<TvShowBase> result = _config.Client.SearchTvShow("Breaking Bad").Result;

            Assert.IsTrue(result.Results.Any(s => s.Id == 1396));
            Assert.IsTrue(result.Results.Any(s => s.Name == "Breaking Bad"));
        }

        [TestMethod]
        public void TestSearchMulti()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchMulti("Arrow", i).Result);

            SearchContainer<SearchMulti> result = _config.Client.SearchMulti("Arrow").Result;
            Assert.IsTrue(result.Results.Any(s => s.Type == MediaType.TVShow && s.Name == "Arrow"));
            Assert.IsTrue(result.Results.Any(s => s.Type == MediaType.Movie && s.Name == "Broken Arrow"));
        }
    }
}
