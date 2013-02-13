using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        public void TestMoviesExtrasExclusive()
        {
            // Test combinations of extra methods, fetch everything but each one, ensure all but the one exist
            foreach (MovieMethods method in _methods.Keys)
            {
                // Prepare the combination exlcuding the one (method).
                MovieMethods combo = _methods.Keys.Except(new[] { method }).Aggregate((movieMethod, accumulator) => movieMethod | accumulator);

                // Fetch data
                Movie movie = _config.Client.GetMovie(AGoodDayToDieHard, extraMethods: combo);

                // Ensure we have all pieces
                foreach (MovieMethods expectedMethod in _methods.Keys.Except(new[] { method }))
                    Assert.IsNotNull(_methods[expectedMethod](movie));

                // .. except the method we're testing.
                Assert.IsNull(_methods[method](movie));
            }
        }

        [TestMethod]
        public void TestMoviesGetters()
        {
            //GetMovieAlternativeTitles(int id, string country)
            {
                AlternativeTitles respUs = _config.Client.GetMovieAlternativeTitles(AGoodDayToDieHard, "US");
                Assert.IsNotNull(respUs);

                AlternativeTitles respGerman = _config.Client.GetMovieAlternativeTitles(AGoodDayToDieHard, "DE");
                Assert.IsNotNull(respGerman);

                Assert.IsFalse(respUs.Titles.Any(s => s.Title == "Stirb Langsam 5"));
                Assert.IsTrue(respGerman.Titles.Any(s => s.Title == "Stirb Langsam 5"));

                Assert.IsTrue(respUs.Titles.All(s => s.Iso_3166_1 == "US"));
                Assert.IsTrue(respGerman.Titles.All(s => s.Iso_3166_1 == "DE"));
            }

            //GetMovieCasts(int id)
            {
                Casts resp = _config.Client.GetMovieCasts(AGoodDayToDieHard);
                Assert.IsNotNull(resp);
            }

            //GetMovieImages(int id, string language)
            {
                ImagesWithId resp = _config.Client.GetMovieImages(AGoodDayToDieHard);
                Assert.IsNotNull(resp);
            }

            //GetMovieKeywords(int id)
            {
                KeywordsContainer resp = _config.Client.GetMovieKeywords(AGoodDayToDieHard);
                Assert.IsNotNull(resp);
            }

            //GetMovieReleases(int id)
            {
                Releases resp = _config.Client.GetMovieReleases(AGoodDayToDieHard);
                Assert.IsNotNull(resp);
            }

            //GetMovieTrailers(int id)
            {
                Trailers resp = _config.Client.GetMovieTrailers(AGoodDayToDieHard);
                Assert.IsNotNull(resp);
            }

            //GetMovieTranslations(int id)
            {
                TranslationsContainer resp = _config.Client.GetMovieTranslations(AGoodDayToDieHard);
                Assert.IsNotNull(resp);
            }

            //GetMovieSimilarMovies(int id, string language, int page = -1)
            {
                SearchContainer<MovieResult> resp = _config.Client.GetMovieSimilarMovies(AGoodDayToDieHard);
                Assert.IsNotNull(resp);

                SearchContainer<MovieResult> respGerman = _config.Client.GetMovieSimilarMovies(AGoodDayToDieHard, language: "de");
                Assert.IsNotNull(respGerman);

                Assert.AreEqual(resp.Results.Count, respGerman.Results.Count);
                Assert.AreEqual(resp.Results.First().Id, respGerman.Results.First().Id);
                Assert.AreNotEqual(resp.Results.First().Title, respGerman.Results.First().Title);
            }

            //GetMovieLists(int id, string language, int page = -1)
            {
                SearchContainer<ListResult> resp = _config.Client.GetMovieLists(AGoodDayToDieHard);
                Assert.IsNotNull(resp);

                SearchContainer<ListResult> respPage2 = _config.Client.GetMovieLists(AGoodDayToDieHard, 2);
                Assert.IsNotNull(respPage2);

                Assert.AreEqual(1, resp.Page);
                Assert.AreEqual(2, respPage2.Page);
                Assert.AreEqual(resp.TotalResults, resp.TotalResults);
            }

            //GetMovieChanges(int id, DateTime? startDate = null, DateTime? endDate = null)
            {
                // Find latest changed title
                int latestChanged = _config.Client.GetMovieLatest().Id;

                // Fetch changelog
                DateTime lower = DateTime.UtcNow.AddDays(-14);
                DateTime higher = DateTime.UtcNow;
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


        }

        [TestMethod]
        public void TestMoviesImages()
        {
            // Get config
            _config.Client.GetConfig();

            // Test image url generator
            ImagesWithId images = _config.Client.GetMovieImages(AGoodDayToDieHard);

            Assert.AreEqual(AGoodDayToDieHard, images.Id);
            Assert.IsTrue(images.Backdrops.Count > 0);
            Assert.IsTrue(images.Posters.Count > 0);

            List<string> backdropSizes = _config.Client.Config.Images.BackdropSizes;
            List<string> posterSizes = _config.Client.Config.Images.PosterSizes;

            foreach (ImageData imageData in images.Backdrops)
            {
                foreach (string size in backdropSizes)
                {
                    Uri url = _config.Client.GetImageUrl(size, imageData.FilePath);
                    Uri urlSecure = _config.Client.GetImageUrl(size, imageData.FilePath, true);

                    Assert.IsTrue(TestHelpers.InternetUriExists(url));
                    Assert.IsTrue(TestHelpers.InternetUriExists(urlSecure));
                }
            }

            foreach (ImageData imageData in images.Posters)
            {
                foreach (string size in posterSizes)
                {
                    Uri url = _config.Client.GetImageUrl(size, imageData.FilePath);
                    Uri urlSecure = _config.Client.GetImageUrl(size, imageData.FilePath, true);

                    Assert.IsTrue(TestHelpers.InternetUriExists(url));
                    Assert.IsTrue(TestHelpers.InternetUriExists(urlSecure));
                }
            }
        }
    }
}
