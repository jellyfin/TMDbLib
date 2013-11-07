using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientSearchTests
    {
        private TestConfig _config;

        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        private void SearchPages<T>(Func<int, SearchContainer<T>> getter)
        {
            // Check page 1
            SearchContainer<T> results = getter(1);

            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Results);
            Assert.AreEqual(1, results.Page);
            Assert.IsTrue(results.Results.Count > 0);
            Assert.IsTrue(results.TotalResults > 0);
            Assert.IsTrue(results.TotalPages > 0);

            // Check page 2
            SearchContainer<T> results2 = getter(2);

            Assert.IsNotNull(results2);
            Assert.IsNotNull(results2.Results);
            Assert.AreEqual(2, results2.Page);
            Assert.AreEqual(results.TotalResults, results2.TotalResults);
            Assert.AreEqual(results.TotalPages, results2.TotalPages);

            if (results.Results.Count == results.TotalResults)
                Assert.AreEqual(0, results2.Results.Count);
            else
                Assert.AreNotEqual(0, results2.Results.Count);

        }

        [TestMethod]
        public void TestSearchMovie()
        {
            // SearchMovie(string query, int page = -1, bool includeAdult = false, int year = -1)
            // SearchMovie(string query, string language, int page = -1, bool includeAdult = false, int year = -1)

            SearchPages(i => _config.Client.SearchMovie("007", i));

            // Search pr. Year
            // 1962: First James Bond movie, "Dr. No"
            SearchContainer<SearchMovie> resultYear = _config.Client.SearchMovie("007", year: 1962);

            Assert.IsNotNull(resultYear);
            Assert.AreEqual(1, resultYear.Page);
            Assert.AreEqual(1, resultYear.Results.Count);
            Assert.AreEqual(1, resultYear.TotalResults);
        }

        [TestMethod]
        public void TestSearchCollection()
        {
            // SearchCollection(string query, int page = -1)
            // SearchCollection(string query, string language, int page = -1)

            SearchPages(i => _config.Client.SearchCollection("007", i));

            // TODO: Extend
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestSearchPerson()
        {
            // SearchPerson(string query, int page = -1, bool includeAdult = false)

            SearchPages(i => _config.Client.SearchPerson("Bruce", i));

            // TODO: Extend
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestSearchList()
        {
            // SearchList(string query, int page = -1, bool includeAdult = false)

            SearchPages(i => _config.Client.SearchList("to watch", i));

            // TODO: Extend
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestSearchCompany()
        {
            // SearchCompany(string query, int page = -1)

            SearchPages(i => _config.Client.SearchCompany("20th", i));

            // TODO: Extend
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestSearchKeyword()
        {
            // SearchKeyword(string query, int page = -1)

            SearchPages(i => _config.Client.SearchKeyword("plot", i));

            // TODO: Extend
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestSearchTvShow()
        {
            // SearchKeyword(string query, int page = -1)

            //SearchPages(i => _config.Client.SearchTvShow("Breaking Bad", i));

            // TODO: Implement
            Assert.Inconclusive();
        }
    }
}
