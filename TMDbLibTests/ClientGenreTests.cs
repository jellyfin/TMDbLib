using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using System.Linq;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientGenreTests
    {
        private TestConfig _config;

        [TestInitialize]
        public void InitTest()
        {
            _config = new TestConfig();
        }

        [TestMethod]
        public void TestGenreList()
        {
            // Default language
            List<Genre> genres = _config.Client.GetGenres();

            Assert.IsNotNull(genres);
            Assert.IsTrue(genres.Count > 0);

            // Another language
            List<Genre> genresDanish = _config.Client.GetGenres("da");

            Assert.IsNotNull(genresDanish);
            Assert.IsTrue(genresDanish.Count > 0);

            Assert.AreEqual(genres.Count, genresDanish.Count);

            // At least one should be different
            Assert.IsTrue(genres.Any(genre => genresDanish.First(danishGenre => danishGenre.Id == genre.Id).Name != genre.Name));
        }

        [TestMethod]
        public void TestGenreMovies()
        {
            // Get first genre
            Genre genre = _config.Client.GetGenres().First();

            // Get movies
            SearchContainerWithId<MovieResult> movies = _config.Client.GetGenreMovies(genre.Id);
            SearchContainerWithId<MovieResult> moviesPage2 = _config.Client.GetGenreMovies(genre.Id, 2);
            SearchContainerWithId<MovieResult> moviesAll = _config.Client.GetGenreMovies(genre.Id, includeAllMovies: true);

            Assert.AreEqual(1, movies.Page);
            Assert.AreEqual(2, moviesPage2.Page);
            Assert.AreEqual(1, moviesAll.Page);

            Assert.IsTrue(movies.Results.Count > 0);
            Assert.IsTrue(moviesPage2.Results.Count > 0);
            Assert.IsTrue(moviesAll.Results.Count > 0);

            Assert.IsTrue(movies.Results.All(s => s != null));
            Assert.IsTrue(moviesPage2.Results.All(s => s != null));
            Assert.IsTrue(moviesAll.Results.All(s => s != null));

            Assert.AreEqual(movies.TotalResults, moviesPage2.TotalResults);
            Assert.IsTrue(moviesAll.TotalResults > movies.TotalResults);
        }
    }
}
