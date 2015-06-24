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