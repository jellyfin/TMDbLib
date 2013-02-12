using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.Movies;
using System.Linq;

namespace TMDbLibTests
{
    [TestClass]
    public class MovieTests
    {
        private const int AGoodDayToDieHard = 47964;

        private Dictionary<MovieMethods, Func<Movie, object>> _methods;

        [TestInitialize]
        public void Initiator()
        {
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
        public void TestExtrasNone()
        {
            Movie movie = GeneralConfig.Client.GetMovie(AGoodDayToDieHard);

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
        public void TestExtrasExclusive()
        {
            // Test combinations of extra methods, fetch everything but each one, ensure all but the one exist
            foreach (MovieMethods method in _methods.Keys)
            {
                // Prepare the combination exlcuding the one (method).
                MovieMethods combo = _methods.Keys.Except(new[] { method }).Aggregate((movieMethod, accumulator) => movieMethod | accumulator);

                // Fetch data
                Movie movie = GeneralConfig.Client.GetMovie(AGoodDayToDieHard, extraMethods: combo);

                // Ensure we have all pieces
                foreach (MovieMethods expectedMethod in _methods.Keys.Except(new[] { method }))
                    Assert.IsNotNull(_methods[expectedMethod](movie));

                // .. except the method we're testing.
                Assert.IsNull(_methods[method](movie));
            }
        }
    }
}
