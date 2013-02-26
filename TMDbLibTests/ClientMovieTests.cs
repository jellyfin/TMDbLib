using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientMovieTests
    {
        private const int AGoodDayToDieHard = 47964;
        private const int Avatar = 19995;

        private Dictionary<MovieMethods, Func<Movie, object>> _methods;
        private TestConfig _config;

        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();

            _methods = new Dictionary<MovieMethods, Func<Movie, object>>();
            _methods[MovieMethods.AlternativeTitles] = movie => movie.AlternativeTitles;
            _methods[MovieMethods.Casts] = movie => movie.Casts;
            _methods[MovieMethods.Images] = movie => movie.Images;
            _methods[MovieMethods.Keywords] = movie => movie.Keywords;
            _methods[MovieMethods.Releases] = movie => movie.Releases;
            _methods[MovieMethods.Trailers] = movie => movie.Trailers;
            _methods[MovieMethods.Translations] = movie => movie.Translations;
            _methods[MovieMethods.SimilarMovies] = movie => movie.SimilarMovies;
            _methods[MovieMethods.Lists] = movie => movie.Lists;
            _methods[MovieMethods.Changes] = movie => movie.Changes;
        }

        [TestMethod]
        public void TestMoviesExtrasNone()
        {
            Movie movie = _config.Client.GetMovie(AGoodDayToDieHard);

            Assert.IsNotNull(movie);

            // TODO: Test all properties
            Assert.AreEqual("A Good Day to Die Hard", movie.Title);

            // Test all extras, ensure none of them exist
            foreach (Func<Movie, object> selector in _methods.Values)
            {
                Assert.IsNull(selector(movie));
            }
        }

        [TestMethod]
        public void TestMoviesExtrasExclusive()
        {
            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => _config.Client.GetMovie(id, extras), AGoodDayToDieHard);
        }

        [TestMethod]
        public void TestMoviesExtrasAll()
        {
            MovieMethods combinedEnum = _methods.Keys.Aggregate((methods, movieMethods) => methods | movieMethods);
            Movie item = _config.Client.GetMovie(AGoodDayToDieHard, combinedEnum);

            TestMethodsHelper.TestGetAll(_methods, item);
        }

        [TestMethod]
        public void TestMoviesNull()
        {
            Movie item = _config.Client.GetMovie(Avatar, "fr");

            Assert.IsNull(item);
        }

        [TestMethod]
        public void TestMoviesLanguage()
        {
            Movie movie = _config.Client.GetMovie(AGoodDayToDieHard);
            Movie movieItalian = _config.Client.GetMovie(AGoodDayToDieHard, "it");

            Assert.IsNotNull(movie);
            Assert.IsNotNull(movieItalian);

            Assert.AreEqual("A Good Day to Die Hard", movie.Title);
            Assert.AreNotEqual(movie.Title, movieItalian.Title);

            // Test all extras, ensure none of them exist
            foreach (Func<Movie, object> selector in _methods.Values)
            {
                Assert.IsNull(selector(movie));
                Assert.IsNull(selector(movieItalian));
            }
        }

        [TestMethod]
        public void TestMoviesGetMovieAlternativeTitles()
        {
            //GetMovieAlternativeTitles(int id, string country)
            AlternativeTitles respUs = _config.Client.GetMovieAlternativeTitles(AGoodDayToDieHard, "US");
            Assert.IsNotNull(respUs);

            AlternativeTitles respGerman = _config.Client.GetMovieAlternativeTitles(AGoodDayToDieHard, "DE");
            Assert.IsNotNull(respGerman);

            Assert.IsFalse(respUs.Titles.Any(s => s.Title == "Stirb Langsam 5"));
            Assert.IsTrue(respGerman.Titles.Any(s => s.Title == "Stirb Langsam 5"));

            Assert.IsTrue(respUs.Titles.All(s => s.Iso_3166_1 == "US"));
            Assert.IsTrue(respGerman.Titles.All(s => s.Iso_3166_1 == "DE"));
        }

        [TestMethod]
        public void TestMoviesGetMovieAlternativeTitlesCountry()
        {
            //GetMovieAlternativeTitles(int id, string country)
            AlternativeTitles respUs = _config.Client.GetMovieAlternativeTitles(AGoodDayToDieHard, "US");
            Assert.IsNotNull(respUs);

            _config.Client.DefaultCountry = "US";

            AlternativeTitles respUs2 = _config.Client.GetMovieAlternativeTitles(AGoodDayToDieHard);
            Assert.IsNotNull(respUs2);

            Assert.AreEqual(respUs.Titles.Count, respUs2.Titles.Count);
        }

        [TestMethod]
        public void TestMoviesGetMovieCasts()
        {
            //GetMovieCasts(int id)
            Casts resp = _config.Client.GetMovieCasts(AGoodDayToDieHard);
            Assert.IsNotNull(resp);
        }

        [TestMethod]
        public void TestMoviesGetMovieImages()
        {
            //GetMovieImages(int id, string language)
            ImagesWithId resp = _config.Client.GetMovieImages(AGoodDayToDieHard);
            Assert.IsNotNull(resp);
        }

        [TestMethod]
        public void TestMoviesGetMovieKeywords()
        {
            //GetMovieKeywords(int id)
            KeywordsContainer resp = _config.Client.GetMovieKeywords(AGoodDayToDieHard);
            Assert.IsNotNull(resp);
        }

        [TestMethod]
        public void TestMoviesGetMovieReleases()
        {
            //GetMovieReleases(int id)
            Releases resp = _config.Client.GetMovieReleases(AGoodDayToDieHard);
            Assert.IsNotNull(resp);
        }

        [TestMethod]
        public void TestMoviesGetMovieTrailers()
        {
            //GetMovieTrailers(int id)
            Trailers resp = _config.Client.GetMovieTrailers(AGoodDayToDieHard);
            Assert.IsNotNull(resp);
        }

        [TestMethod]
        public void TestMoviesGetMovieTranslations()
        {
            //GetMovieTranslations(int id)
            TranslationsContainer resp = _config.Client.GetMovieTranslations(AGoodDayToDieHard);
            Assert.IsNotNull(resp);
        }

        [TestMethod]
        public void TestMoviesGetMovieSimilarMovies()
        {
            //GetMovieSimilarMovies(int id, string language, int page = -1)
            SearchContainer<MovieResult> resp = _config.Client.GetMovieSimilarMovies(AGoodDayToDieHard);
            Assert.IsNotNull(resp);

            SearchContainer<MovieResult> respGerman = _config.Client.GetMovieSimilarMovies(AGoodDayToDieHard, language: "de");
            Assert.IsNotNull(respGerman);

            Assert.AreEqual(resp.Results.Count, respGerman.Results.Count);
            Assert.AreEqual(resp.Results.First().Id, respGerman.Results.First().Id);
            Assert.AreNotEqual(resp.Results.First().Title, respGerman.Results.First().Title);
        }

        [TestMethod]
        public void TestMoviesGetMovieLists()
        {
            //GetMovieLists(int id, string language, int page = -1)
            SearchContainer<ListResult> resp = _config.Client.GetMovieLists(AGoodDayToDieHard);
            Assert.IsNotNull(resp);

            SearchContainer<ListResult> respPage2 = _config.Client.GetMovieLists(AGoodDayToDieHard, 2);
            Assert.IsNotNull(respPage2);

            Assert.AreEqual(1, resp.Page);
            Assert.AreEqual(2, respPage2.Page);
            Assert.AreEqual(resp.TotalResults, resp.TotalResults);
        }

        [TestMethod]
        public void TestMoviesGetMovieChanges()
        {
            //GetMovieChanges(int id, DateTime? startDate = null, DateTime? endDate = null)
            // Find latest changed title
            int latestChanged = _config.Client.GetMovieLatest().Id;

            // Fetch changelog
            DateTime lower = DateTime.UtcNow.AddDays(-13);
            DateTime higher = DateTime.UtcNow.AddDays(1);
            List<Change> respRange = _config.Client.GetMovieChanges(latestChanged, lower, higher);

            Assert.IsNotNull(respRange);
            Assert.IsTrue(respRange.Count > 0);

            // As TMDb works in days, we need to adjust our values also
            lower = lower.AddDays(-1);
            higher = higher.AddDays(1);

            foreach (Change change in respRange)
                foreach (ChangeItem changeItem in change.Items)
                {
                    DateTime date = changeItem.TimeParsed;
                    Assert.IsTrue(lower <= date);
                    Assert.IsTrue(date <= higher);
                }
        }

        [TestMethod]
        public void TestMoviesImages()
        {
            // Get config
            _config.Client.GetConfig();

            // Test image url generator
            ImagesWithId images = _config.Client.GetMovieImages(AGoodDayToDieHard);

            Assert.AreEqual(AGoodDayToDieHard, images.Id);
            TestImagesHelpers.TestImages(_config, images);
        }

        [TestMethod]
        public void TestMoviesList()
        {
            //GetMovieList(MovieListType type, string language, int page = -1)
            foreach (MovieListType type in Enum.GetValues(typeof(MovieListType)).OfType<MovieListType>())
            {
                SearchContainer<MovieResult> list = _config.Client.GetMovieList(MovieListType.NowPlaying);

                Assert.IsNotNull(list);
                Assert.IsTrue(list.Results.Count > 0);
                Assert.AreEqual(1, list.Page);

                SearchContainer<MovieResult> listPage2 = _config.Client.GetMovieList(MovieListType.NowPlaying, 2);

                Assert.IsNotNull(listPage2);
                Assert.IsTrue(listPage2.Results.Count > 0);
                Assert.AreEqual(2, listPage2.Page);

                Assert.AreEqual(list.TotalResults, listPage2.TotalResults);

                SearchContainer<MovieResult> listDe = _config.Client.GetMovieList(MovieListType.NowPlaying, "de");

                Assert.IsNotNull(listDe);
                Assert.IsTrue(listDe.Results.Count > 0);
                Assert.AreEqual(1, listDe.Page);

                // At least one title should differ
                Assert.IsTrue(list.Results.Any(s => listDe.Results.Any(x => x.Title != s.Title)));
            }
        }
    }
}
