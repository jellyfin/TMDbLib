using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using System.Linq;

namespace TMDbLibTests
{
    [TestClass]
    public class MovieTests
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
        public void TestMoviesImages()
        {
            // Get config
            _config.Client.GetConfig();

            // Test image url generator
            ImagesWithId images = _config.Client.GetMovieImages(AGoodDayToDieHard);

            Assert.AreEqual(AGoodDayToDieHard, images.Id);
            Assert.IsTrue(images.Backdrops.Count > 0);
            Assert.IsTrue(images.Posters.Count > 0);

            var backdropSizes = _config.Client.Config.Images.BackdropSizes;
            var posterSizes = _config.Client.Config.Images.PosterSizes;

            foreach (ImageData imageData in images.Backdrops)
            {
                foreach (string size in backdropSizes)
                {
                    Uri url = _config.Client.GetImageUrl(size, imageData.FilePath);
                    Uri urlSecure = _config.Client.GetImageUrl(size, imageData.FilePath, true);

                    Assert.IsTrue(InternetUriExists(url));
                    Assert.IsTrue(InternetUriExists(urlSecure));
                }
            }

            foreach (ImageData imageData in images.Posters)
            {
                foreach (string size in posterSizes)
                {
                    Uri url = _config.Client.GetImageUrl(size, imageData.FilePath);
                    Uri urlSecure = _config.Client.GetImageUrl(size, imageData.FilePath, true);

                    Assert.IsTrue(InternetUriExists(url));
                    Assert.IsTrue(InternetUriExists(urlSecure));
                }
            }
        }

        private bool InternetUriExists(Uri uri)
        {
            HttpWebRequest req = HttpWebRequest.Create(uri) as HttpWebRequest;
            req.Method = "HEAD";

            try
            {
                req.GetResponse();
                // It exists
                return true;
            }
            catch (WebException ex)
            {
                Debug.WriteLine(((HttpWebResponse) ex.Response).StatusCode + ": " + uri);
                return false;
            }
        }
    }
}
