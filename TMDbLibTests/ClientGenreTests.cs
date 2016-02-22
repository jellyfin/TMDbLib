using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientGenreTests
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
        public void TestGenreTvList()
        {
            // Default language
            List<Genre> genres = _config.Client.GetTvGenresAsync().Result;

            Assert.IsNotNull(genres);
            Assert.IsTrue(genres.Count > 0);

            // Another language
            List<Genre> genresDanish = _config.Client.GetTvGenresAsync("da").Result;

            Assert.IsNotNull(genresDanish);
            Assert.IsTrue(genresDanish.Count > 0);

            Assert.AreEqual(genres.Count, genresDanish.Count);

            // At least one should be different
            Assert.IsTrue(genres.Any(genre => genresDanish.First(danishGenre => danishGenre.Id == genre.Id).Name != genre.Name));
        }

        [TestMethod]
        public void TestGenreMovieList()
        {
            // Default language
            List<Genre> genres = _config.Client.GetMovieGenresAsync().Result;

            Assert.IsNotNull(genres);
            Assert.IsTrue(genres.Count > 0);

            // Another language
            List<Genre> genresDanish = _config.Client.GetMovieGenresAsync("da").Result;

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
            Genre genre = _config.Client.GetMovieGenresAsync().Result.First();

            // Get movies
            SearchContainerWithId<MovieResult> movies = _config.Client.GetGenreMoviesAsync(genre.Id).Result;
            SearchContainerWithId<MovieResult> moviesPage2 = _config.Client.GetGenreMoviesAsync(genre.Id, "it", 2, includeAllMovies: false).Result;
            SearchContainerWithId<MovieResult> moviesAll = _config.Client.GetGenreMoviesAsync(genre.Id, includeAllMovies: true).Result;

            Assert.AreEqual(1, movies.Page);
            Assert.AreEqual(2, moviesPage2.Page);
            Assert.AreEqual(1, moviesAll.Page);

            Assert.IsTrue(movies.Results.Count > 0);
            Assert.IsTrue(moviesPage2.Results.Count > 0);
            Assert.IsTrue(moviesAll.Results.Count > 0);

            Assert.IsTrue(movies.Results.All(s => s != null));
            Assert.IsTrue(moviesPage2.Results.All(s => s != null));
            Assert.IsTrue(moviesAll.Results.All(s => s != null));

            Assert.AreEqual(movies.TotalResults, moviesPage2.TotalResults);     // Should be the same, despite the use of 'includeAllMovies' and Italian
            Assert.IsTrue(moviesAll.TotalResults >= movies.TotalResults);
        }
    }
}
