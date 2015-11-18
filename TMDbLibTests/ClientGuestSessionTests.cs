using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientGuestSessionTests
    {
        private const int Avatar = 19995;

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
        public void TestMoviesSetRatingGuestSession()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.IsTrue(_config.Client.MovieSetRating(Avatar, 7.5).Result);

            SearchContainer<MovieWithRating> ratings = _config.Client.GetGuestSessionRatedMovies().Result;

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            Assert.IsTrue(ratings.Results.Any(s => s.Id == Avatar));
            Assert.AreEqual(7.5, ratings.Results.Single(s => s.Id == Avatar).Rating, float.Epsilon);

            // Try changing it back to the previous rating
            Assert.IsTrue(_config.Client.MovieSetRating(Avatar, 8).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedMovies().Result;

            Assert.IsTrue(ratings.Results.Any(s => s.Id == Avatar));
            Assert.AreEqual(8, ratings.Results.Single(s => s.Id == Avatar).Rating, float.Epsilon);
        }

        [TestMethod]
        public void TestGuestSessionGetRatedMovies()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Test paging
            TestHelpers.SearchPages(i => _config.Client.GetGuestSessionRatedMovies(i).Result);

            // Fetch ratings
            SearchContainer<MovieWithRating> result = _config.Client.GetGuestSessionRatedMovies().Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Results);
        }
    }
}