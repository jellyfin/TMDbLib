using System;
using System.Linq;
using System.Threading;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientGuestSessionTests : TestBase
    {
        private readonly TestConfig _config;

        public ClientGuestSessionTests()
        {
            _config = new TestConfig();
        }

        [Fact]
        public void TestTvEpisodeSetRatingGuestSession()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(_config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 7.5).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            SearchContainer<TvEpisodeWithRating> ratings = _config.Client.GetGuestSessionRatedTvEpisodesAsync().Sync();

            double tmpRating = ratings.Results.Single(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1).Rating;
            Assert.True(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
            Assert.True(Math.Abs(7.5 - tmpRating) < float.Epsilon);

            // Try changing it back to the previous rating
            Assert.True(_config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 8).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedTvEpisodesAsync().Sync();

            tmpRating = ratings.Results.Single(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1).Rating;
            Assert.True(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
            Assert.True(Math.Abs(8 - tmpRating) < float.Epsilon);

            // Try removing the rating
            Assert.True(_config.Client.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedTvEpisodesAsync().Sync();

            Assert.False(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
        }

        [Fact]
        public void TestTvSetRatingGuestSession()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(_config.Client.TvShowSetRatingAsync(IdHelper.House, 7.5).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            SearchContainer<TvShowWithRating> ratings = _config.Client.GetGuestSessionRatedTvAsync().Sync();

            double tmpRating = ratings.Results.Single(s => s.Id == IdHelper.House).Rating;
            Assert.True(ratings.Results.Any(s => s.Id == IdHelper.House));
            Assert.True(Math.Abs(7.5 - tmpRating) < float.Epsilon);

            // Try changing it back to the previous rating
            Assert.True(_config.Client.TvShowSetRatingAsync(IdHelper.House, 8).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedTvAsync().Sync();

            tmpRating = ratings.Results.Single(s => s.Id == IdHelper.House).Rating;
            Assert.True(ratings.Results.Any(s => s.Id == IdHelper.House));
            Assert.True(Math.Abs(8 - tmpRating) < float.Epsilon);

            // Try removing the rating
            Assert.True(_config.Client.TvShowRemoveRatingAsync(IdHelper.House).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedTvAsync().Sync();

            Assert.False(ratings.Results.Any(s => s.Id == IdHelper.House));
        }

        [Fact]
        public void TestMoviesSetRatingGuestSession()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(_config.Client.MovieSetRatingAsync(IdHelper.Avatar, 7.5).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            SearchContainer<MovieWithRating> ratings = _config.Client.GetGuestSessionRatedMoviesAsync().Sync();

            double tmpRating = ratings.Results.Single(s => s.Id == IdHelper.Avatar).Rating;
            Assert.True(ratings.Results.Any(s => s.Id == IdHelper.Avatar));
            Assert.True(Math.Abs(7.5 - tmpRating) < float.Epsilon);

            // Try changing it back to the previous rating
            Assert.True(_config.Client.MovieSetRatingAsync(IdHelper.Avatar, 8).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedMoviesAsync().Sync();

            tmpRating = ratings.Results.Single(s => s.Id == IdHelper.Avatar).Rating;
            Assert.True(ratings.Results.Any(s => s.Id == IdHelper.Avatar));
            Assert.True(Math.Abs(8 - tmpRating) < float.Epsilon);

            // Try removing the rating
            Assert.True(_config.Client.MovieRemoveRatingAsync(IdHelper.Avatar).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = _config.Client.GetGuestSessionRatedMoviesAsync().Sync();

            Assert.False(ratings.Results.Any(s => s.Id == IdHelper.Avatar));
        }

        [Fact]
        public void TestGuestSessionGetRatedTvEpisodes()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.True(_config.Client.TvEpisodeSetRatingAsync(IdHelper.BigBangTheory, 1, 1, 7.5).Result);

            // Test paging
            TestHelpers.SearchPages(i => _config.Client.GetGuestSessionRatedTvEpisodesAsync(i).Result);

            // Fetch ratings
            SearchContainer<TvEpisodeWithRating> result = _config.Client.GetGuestSessionRatedTvEpisodesAsync().Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
        }

        [Fact]
        public void TestGuestSessionGetRatedTv()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.True(_config.Client.TvShowSetRatingAsync(IdHelper.BigBangTheory, 7.5).Result);

            // Test paging
            TestHelpers.SearchPages(i => _config.Client.GetGuestSessionRatedTvAsync(i).Result);

            // Fetch ratings
            SearchContainer<TvShowWithRating> result = _config.Client.GetGuestSessionRatedTvAsync().Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
        }

        [Fact]
        public void TestGuestSessionGetRatedMovies()
        {
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.True(_config.Client.MovieSetRatingAsync(IdHelper.Terminator, 7.5).Result);

            // Test paging
            TestHelpers.SearchPages(i => _config.Client.GetGuestSessionRatedMoviesAsync(i).Result);

            // Fetch ratings
            SearchContainer<MovieWithRating> result = _config.Client.GetGuestSessionRatedMoviesAsync().Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
        }
    }
}