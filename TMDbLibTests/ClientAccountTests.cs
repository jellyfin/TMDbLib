using System;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientAccountTests
    {
        private TestConfig _config;
        private const int Terminator = 218;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();

            if (string.IsNullOrWhiteSpace(_config.UserSessionId))
                throw new ConfigurationErrorsException("To successfully complete the ClientAccountTests you will need to specify a valid 'UserSessionId' in the test config file");
        }

        [TestMethod]
        [ExpectedException(typeof(UserSessionRequiredException))]
        public void TestAccountGetDetailsGuestAccount()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);
            AccountDetails account = _config.Client.AccountGetDetails().Result;

            // Should always throw exception
            Assert.Fail();
        }

        [TestMethod]
        public void TestAccountGetDetailsUserAccount()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            AccountDetails account = _config.Client.AccountGetDetails().Result;

            // Naturally the specified account must have these values populated for the test to pass
            Assert.IsNotNull(account);
            Assert.IsTrue(account.Id > 1);
            Assert.AreEqual("Test Name", account.Name);
            Assert.AreEqual("TMDbTestAccount", account.Username);
            Assert.AreEqual("BE", account.Iso_3166_1);
            Assert.AreEqual("en", account.Iso_639_1);
        }

        [TestMethod]
        public void TestAccountAccountGetLists()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => _config.Client.AccountGetLists(i).Result);
            List list = _config.Client.AccountGetLists().Result.Results[0];

            Assert.IsNotNull(list.Id);
            Assert.IsNotNull(list.Name);
            Assert.IsNotNull(list.PosterPath);
            Assert.IsNotNull(list.Description);
            Assert.IsNotNull(list.ListType);
            Assert.IsNotNull(list.Iso_639_1);
            Assert.IsNull(list.Items);
            Assert.IsNull(list.CreatedBy);
        }

        [TestMethod]
        public void TestAccountGetFavoriteMovies()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => _config.Client.AccountGetFavoriteMovies(i).Result);
            SearchMovie movie = _config.Client.AccountGetFavoriteMovies().Result.Results[0];

            // Requires that you have marked at least one movie as favorite else this test will fail
            Assert.IsTrue(movie.Id > 0);
            Assert.IsNotNull(movie.Title);
            Assert.IsNotNull(movie.PosterPath);
            Assert.IsNotNull(movie.BackdropPath);
            Assert.IsNotNull(movie.OriginalTitle);
            Assert.IsNotNull(movie.ReleaseDate);
            Assert.IsTrue(movie.VoteCount > 0);
            Assert.IsTrue(movie.VoteAverage > 0);
            Assert.IsTrue(movie.Popularity > 0);
        }

        [TestMethod]
        public void TestAccountGetMovieWatchlist()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => _config.Client.AccountGetFavoriteMovies(i).Result);
            SearchMovie movie = _config.Client.AccountGetFavoriteMovies().Result.Results[0];

            // Requires that you have added at least one movie to your watchlist else this test will fail
            Assert.IsTrue(movie.Id > 0);
            Assert.IsNotNull(movie.Title);
            Assert.IsNotNull(movie.PosterPath);
            Assert.IsNotNull(movie.BackdropPath);
            Assert.IsNotNull(movie.OriginalTitle);
            Assert.IsNotNull(movie.ReleaseDate);
            Assert.IsTrue(movie.VoteCount > 0);
            Assert.IsTrue(movie.VoteAverage > 0);
            Assert.IsTrue(movie.Popularity > 0);
        }

        [TestMethod]
        public void TestAccountGetRatedMovies()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => _config.Client.AccountGetFavoriteMovies(i).Result);
            SearchMovie movie = _config.Client.AccountGetFavoriteMovies().Result.Results[0];

            // Requires that you have rated at least one movie else this test will fail
            Assert.IsTrue(movie.Id > 0);
            Assert.IsNotNull(movie.Title);
            Assert.IsNotNull(movie.PosterPath);
            Assert.IsNotNull(movie.BackdropPath);
            Assert.IsNotNull(movie.OriginalTitle);
            Assert.IsNotNull(movie.ReleaseDate);
            Assert.IsTrue(movie.VoteCount > 0);
            Assert.IsTrue(movie.VoteAverage > 0);
            Assert.IsTrue(movie.Popularity > 0);
        }

        [TestMethod]
        public void TestAccountChangeMovieFavoriteStatus()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            // Ensure that the test movie is not marked as favorite before we start the test
            if (DoesFavoriteListContainSpecificMovie(Terminator))
                Assert.Fail("Test movie '{0}' was already marked as favorite unable to perform test correctly", Terminator);

            // Try to mark is as a favorite
            Assert.IsTrue(_config.Client.AccountChangeMovieFavoriteStatus(Terminator, true).Result);

            // Check if it worked
            Assert.IsTrue(DoesFavoriteListContainSpecificMovie(Terminator));

            // Try to un-mark is as a favorite
            Assert.IsTrue(_config.Client.AccountChangeMovieFavoriteStatus(Terminator, false).Result);

            // Check if it worked
            Assert.IsFalse(DoesFavoriteListContainSpecificMovie(Terminator));
        }

        [TestMethod]
        public void TestAccountChangeMovieWatchlistStatus()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            // Ensure that the test movie is not marked as favorite before we start the test
            if (DoesWatchListContainSpecificMovie(Terminator))
                Assert.Fail("Test movie '{0}' was already on watchlist unable to perform test correctly", Terminator);

            // Try to add an item to the watchlist
            Assert.IsTrue(_config.Client.AccountChangeMovieWatchlistStatus(Terminator, true).Result);

            // Check if it worked
            Assert.IsTrue(DoesWatchListContainSpecificMovie(Terminator));

            // Try to remove item from watchlist
            Assert.IsTrue(_config.Client.AccountChangeMovieWatchlistStatus(Terminator, false).Result);

            // Check if it worked
            Assert.IsFalse(DoesWatchListContainSpecificMovie(Terminator));
        }

        private bool DoesFavoriteListContainSpecificMovie(int movieId)
        {
            return DoesListContainSpecificMovie(movieId, page => _config.Client.AccountGetFavoriteMovies(page).Result);
        }

        private bool DoesWatchListContainSpecificMovie(int movieId)
        {
            return DoesListContainSpecificMovie(movieId, page => _config.Client.AccountGetMovieWatchlist(page).Result);
        }

        private bool DoesListContainSpecificMovie(int movieId, Func<int, SearchContainer<SearchMovie>> listGetter)
        {
            int page = 1;
            SearchContainer<SearchMovie> originalList = listGetter(1);
            while (originalList != null && originalList.Results != null && originalList.Results.Any())
            {
                // Check if the current result page contains the relevant movie
                if (originalList.Results.Any(m => m.Id == movieId))
                    return true;

                // See if there is an other page we could try, if not the test passes
                originalList = originalList.Page < originalList.TotalPages ? listGetter(++page) : null;
            }
            return false;
        }
    }
}
