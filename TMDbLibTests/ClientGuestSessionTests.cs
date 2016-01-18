using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;
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
        public void TestTvEpisodeSetRatingGuestSession()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.IsTrue(_config.Client.TvEpisodeSetRating(IdHelper.BreakingBad, 1, 1, 7.5).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            SearchContainer<TvEpisodeWithRating> ratings = _config.Client.GetGuestSessionRatedTvEpisodes().Result;

            Assert.IsTrue(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
            Assert.AreEqual(7.5, ratings.Results.Single(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1).Rating, float.Epsilon);

            // Try changing it back to the previous rating
            Assert.IsTrue(_config.Client.TvEpisodeSetRating(IdHelper.BreakingBad, 1, 1, 8).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedTvEpisodes().Result;

            Assert.IsTrue(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
            Assert.AreEqual(8, ratings.Results.Single(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1).Rating, float.Epsilon);

            // Try removing the rating
            Assert.IsTrue(_config.Client.TvEpisodeRemoveRating(IdHelper.BreakingBad, 1, 1).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedTvEpisodes().Result;

            Assert.IsFalse(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
        }

        [TestMethod]
        public void TestTvSetRatingGuestSession()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.IsTrue(_config.Client.TvShowSetRating(IdHelper.House, 7.5).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            SearchContainer<TvShowWithRating> ratings = _config.Client.GetGuestSessionRatedTv().Result;

            Assert.IsTrue(ratings.Results.Any(s => s.Id == IdHelper.House));
            Assert.AreEqual(7.5, ratings.Results.Single(s => s.Id == IdHelper.House).Rating, float.Epsilon);

            // Try changing it back to the previous rating
            Assert.IsTrue(_config.Client.TvShowSetRating(IdHelper.House, 8).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedTv().Result;

            Assert.IsTrue(ratings.Results.Any(s => s.Id == IdHelper.House));
            Assert.AreEqual(8, ratings.Results.Single(s => s.Id == IdHelper.House).Rating, float.Epsilon);

            // Try removing the rating
            Assert.IsTrue(_config.Client.TvShowRemoveRating(IdHelper.House).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedTv().Result;

            Assert.IsFalse(ratings.Results.Any(s => s.Id == IdHelper.House));
        }

        [TestMethod]
        public void TestMoviesSetRatingGuestSession()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.IsTrue(_config.Client.MovieSetRating(IdHelper.Avatar, 7.5).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            SearchContainer<MovieWithRating> ratings = _config.Client.GetGuestSessionRatedMovies().Result;

            Assert.IsTrue(ratings.Results.Any(s => s.Id == IdHelper.Avatar));
            Assert.AreEqual(7.5, ratings.Results.Single(s => s.Id == IdHelper.Avatar).Rating, float.Epsilon);

            // Try changing it back to the previous rating
            Assert.IsTrue(_config.Client.MovieSetRating(IdHelper.Avatar, 8).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedMovies().Result;

            Assert.IsTrue(ratings.Results.Any(s => s.Id == IdHelper.Avatar));
            Assert.AreEqual(8, ratings.Results.Single(s => s.Id == IdHelper.Avatar).Rating, float.Epsilon);

            // Try removing the rating
            Assert.IsTrue(_config.Client.MovieRemoveRating(IdHelper.Avatar).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedMovies().Result;

            Assert.IsFalse(ratings.Results.Any(s => s.Id == IdHelper.Avatar));
        }

        [TestMethod]
        public void TestGuestSessionGetRatedTvEpisodes()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.IsTrue(_config.Client.TvEpisodeSetRating(IdHelper.BigBangTheory, 1, 1, 7.5).Result);
            
            // Test paging
            TestHelpers.SearchPages(i => _config.Client.GetGuestSessionRatedTvEpisodes(i).Result);

            // Fetch ratings
            SearchContainer<TvEpisodeWithRating> result = _config.Client.GetGuestSessionRatedTvEpisodes().Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Results);
        }

        [TestMethod]
        public void TestGuestSessionGetRatedTv()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.IsTrue(_config.Client.TvShowSetRating(IdHelper.BigBangTheory, 7.5).Result);
            
            // Test paging
            TestHelpers.SearchPages(i => _config.Client.GetGuestSessionRatedTv(i).Result);

            // Fetch ratings
            SearchContainer<TvShowWithRating> result = _config.Client.GetGuestSessionRatedTv().Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Results);
        }

        [TestMethod]
        public void TestGuestSessionGetRatedMovies()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.IsTrue(_config.Client.MovieSetRating(IdHelper.Terminator, 7.5).Result);
            
            // Test paging
            TestHelpers.SearchPages(i => _config.Client.GetGuestSessionRatedMovies(i).Result);

            // Fetch ratings
            SearchContainer<MovieWithRating> result = _config.Client.GetGuestSessionRatedMovies().Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Results);
        }
    }
}