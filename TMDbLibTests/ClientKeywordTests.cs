using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientKeywordTests
    {
        private const int AGoodDayToDieHard = 47964;
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
        public void TestKeywordGet()
        {
            KeywordsContainer keywords = _config.Client.GetMovieKeywords(AGoodDayToDieHard).Result;

            Assert.IsNotNull(keywords);
            Assert.IsNotNull(keywords.Keywords);
            Assert.IsTrue(keywords.Keywords.Count > 0);

            // Try to get all keywords
            foreach (Keyword testKeyword in keywords.Keywords)
            {
                Keyword getKeyword = _config.Client.GetKeyword(testKeyword.Id).Result;

                Assert.IsNotNull(getKeyword);

                Assert.AreEqual(testKeyword.Id, getKeyword.Id);
                Assert.AreEqual(testKeyword.Name, getKeyword.Name);
            }
        }

        [TestMethod]
        public void TestKeywordMovies()
        {
            KeywordsContainer keywords = _config.Client.GetMovieKeywords(AGoodDayToDieHard).Result;

            Assert.IsNotNull(keywords);
            Assert.IsNotNull(keywords.Keywords);
            Assert.IsTrue(keywords.Keywords.Count > 0);

            // Get first keyword
            Keyword testKeyword = keywords.Keywords.First();

            // Get movies
            SearchContainer<MovieResult> movies = _config.Client.GetKeywordMovies(testKeyword.Id).Result;
            SearchContainer<MovieResult> moviesItalian = _config.Client.GetKeywordMovies(testKeyword.Id, "it").Result;
            SearchContainer<MovieResult> moviesPage2 = _config.Client.GetKeywordMovies(testKeyword.Id, 2).Result;

            Assert.IsNotNull(movies);
            Assert.IsNotNull(moviesItalian);
            Assert.IsNotNull(moviesPage2);

            Assert.IsTrue(movies.Results.Count > 0);
            Assert.IsTrue(moviesItalian.Results.Count > 0);

            if (movies.TotalResults > movies.Results.Count)
                Assert.IsTrue(moviesPage2.Results.Count > 0);
            else
                Assert.AreEqual(0, moviesPage2.Results.Count);

            Assert.AreEqual(1, movies.Page);
            Assert.AreEqual(1, moviesItalian.Page);
            Assert.AreEqual(2, moviesPage2.Page);

            // All titles on page 1 must be the same
            bool allTitlesIdentical = true;
            for (int index = 0; index < movies.Results.Count; index++)
            {
                Assert.AreEqual(movies.Results[index].Id, moviesItalian.Results[index].Id);

                // At least one title must differ in title
                if (movies.Results[index].Title != moviesItalian.Results[index].Title)
                    allTitlesIdentical = false;
            }

            Assert.IsFalse(allTitlesIdentical);
        }
    }
}
