using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using System.Linq;
using TMDbLib.Objects.TvShows;

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
            SearchPages(i => _config.Client.SearchMovie("007", i));

            // Search pr. Year
            // 1962: First James Bond movie, "Dr. No"
            SearchContainer<SearchMovie> result = _config.Client.SearchMovie("007", year: 1962);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Page);
            Assert.AreEqual(1, result.Results.Count);
            Assert.AreEqual(1, result.TotalResults);
        }

        [TestMethod]
        public void TestSearchCollection()
        {
            SearchPages(i => _config.Client.SearchCollection("007", i));

            SearchContainer<SearchResultCollection> result = _config.Client.SearchCollection("James Bond");

            Debug.Assert(result.Results.Any(s => s.Id == 645));
            Debug.Assert(result.Results.Any(s => s.Name == "James Bond Collection"));
        }

        [TestMethod]
        public void TestSearchPerson()
        {
            SearchPages(i => _config.Client.SearchPerson("Bruce", i));

            SearchContainer<SearchPerson> result = _config.Client.SearchPerson("Bruce");

            Debug.Assert(result.Results.Any(s => s.Id == 62));
            Debug.Assert(result.Results.Any(s => s.Name == "Bruce Willis"));
        }

        [TestMethod]
        public void TestSearchList()
        {
            SearchPages(i => _config.Client.SearchList("to watch", i));

            SearchContainer<SearchList> result = _config.Client.SearchList("2013");

            Debug.Assert(result.Results.Any(s => s.Id == "50cbe90b19c2956de8047b4f"));
            Debug.Assert(result.Results.Any(s => s.Name == "Sci-Fi films to see in 2013"));
        }

        [TestMethod]
        public void TestSearchCompany()
        {
            SearchPages(i => _config.Client.SearchCompany("20th", i));

            SearchContainer<SearchCompany> result = _config.Client.SearchCompany("20th");

            Debug.Assert(result.Results.Any(s => s.Id == 25));
            Debug.Assert(result.Results.Any(s => s.Name == "20th Century Fox"));
        }

        [TestMethod]
        public void TestSearchKeyword()
        {
            SearchPages(i => _config.Client.SearchKeyword("plot", i));

            SearchContainer<SearchKeyword> result = _config.Client.SearchKeyword("plot");

            Debug.Assert(result.Results.Any(s => s.Id == 11422));
            Debug.Assert(result.Results.Any(s => s.Name == "plot twist"));
        }

        [TestMethod]
        public void TestSearchTvShow()
        {
            SearchPages(i => _config.Client.SearchTvShow("Breaking Bad", i));

            SearchContainer<TvShowBase> result = _config.Client.SearchTvShow("Breaking Bad");

            Debug.Assert(result.Results.Any(s => s.Id == 1396));
            Debug.Assert(result.Results.Any(s => s.Name == "Breaking Bad"));
        }
    }
}
