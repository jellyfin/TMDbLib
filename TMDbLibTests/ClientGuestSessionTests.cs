using System;
using System.Linq;
using System.Threading;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientGuestSessionTests : TestBase
    {
        public ClientGuestSessionTests(TestConfig testConfig) : base(testConfig)
        {
        }

        [Fact]
        public void TestTvEpisodeSetRatingGuestSession()
        {
            Config.Client.SetSessionInformation(Config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 7.5).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            SearchContainer<TvEpisodeWithRating> ratings = Config.Client.GetGuestSessionRatedTvEpisodesAsync().Sync();

            Assert.False(true, "This test has been failing for some time - TMDb has been made aware, but have not responded");

            //double tmpRating = ratings.Results.Single(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1).Rating;
            //Assert.True(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
            //Assert.True(Math.Abs(7.5 - tmpRating) < float.Epsilon);

            //// Try changing it back to the previous rating
            //Assert.True(Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 8).Result);

            //// Allow TMDb to cache our changes
            //Thread.Sleep(2000);

            //ratings = Config.Client.GetGuestSessionRatedTvEpisodesAsync().Sync();

            //tmpRating = ratings.Results.Single(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1).Rating;
            //Assert.True(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
            //Assert.True(Math.Abs(8 - tmpRating) < float.Epsilon);

            //// Try removing the rating
            //Assert.True(Config.Client.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1).Result);

            //// Allow TMDb to cache our changes
            //Thread.Sleep(2000);

            //ratings = Config.Client.GetGuestSessionRatedTvEpisodesAsync().Sync();

            //Assert.False(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
        }

        [Fact]
        public void TestTvSetRatingGuestSession()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(Config.Client.TvShowSetRatingAsync(IdHelper.House, 7.5).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            SearchContainer<SearchTvShowWithRating> ratings = Config.Client.GetGuestSessionRatedTvAsync().Sync();

            double tmpRating = ratings.Results.Single(s => s.Id == IdHelper.House).Rating;
            Assert.True(ratings.Results.Any(s => s.Id == IdHelper.House));
            Assert.True(Math.Abs(7.5 - tmpRating) < float.Epsilon);

            // Try changing it back to the previous rating
            Assert.True(Config.Client.TvShowSetRatingAsync(IdHelper.House, 8).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = Config.Client.GetGuestSessionRatedTvAsync().Sync();

            tmpRating = ratings.Results.Single(s => s.Id == IdHelper.House).Rating;
            Assert.True(ratings.Results.Any(s => s.Id == IdHelper.House));
            Assert.True(Math.Abs(8 - tmpRating) < float.Epsilon);

            // Try removing the rating
            Assert.True(Config.Client.TvShowRemoveRatingAsync(IdHelper.House).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = Config.Client.GetGuestSessionRatedTvAsync().Sync();

            Assert.False(ratings.Results.Any(s => s.Id == IdHelper.House));
        }

        [Fact]
        public void TestMoviesSetRatingGuestSession()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(Config.Client.MovieSetRatingAsync(IdHelper.Terminator, 7.5).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            SearchContainer<SearchMovieWithRating> ratings = Config.Client.GetGuestSessionRatedMoviesAsync().Sync();

            double tmpRating = ratings.Results.Single(s => s.Id == IdHelper.Terminator).Rating;
            Assert.True(ratings.Results.Any(s => s.Id == IdHelper.Terminator));
            Assert.True(Math.Abs(7.5 - tmpRating) < float.Epsilon);

            // Try changing it back to the previous rating
            Assert.True(Config.Client.MovieSetRatingAsync(IdHelper.Terminator, 8).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = Config.Client.GetGuestSessionRatedMoviesAsync().Sync();

            tmpRating = ratings.Results.Single(s => s.Id == IdHelper.Terminator).Rating;
            Assert.True(ratings.Results.Any(s => s.Id == IdHelper.Terminator));
            Assert.True(Math.Abs(8 - tmpRating) < float.Epsilon);

            // Try removing the rating
            Assert.True(Config.Client.MovieRemoveRatingAsync(IdHelper.Terminator).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            ratings = Config.Client.GetGuestSessionRatedMoviesAsync().Sync();

            Assert.False(ratings.Results.Any(s => s.Id == IdHelper.Terminator));
        }

        [Fact]
        public void TestGuestSessionGetRatedTvEpisodes()
        {
            Config.Client.SetSessionInformation(Config.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.True(Config.Client.TvEpisodeSetRatingAsync(IdHelper.BigBangTheory, 1, 1, 7.5).Result);

            // Test paging
            TestHelpers.SearchPages(i => Config.Client.GetGuestSessionRatedTvEpisodesAsync(i).Result);

            // Fetch ratings
            SearchContainer<TvEpisodeWithRating> result = Config.Client.GetGuestSessionRatedTvEpisodesAsync().Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
        }

        [Fact]
        public void TestGuestSessionGetRatedTv()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.True(Config.Client.TvShowSetRatingAsync(IdHelper.BigBangTheory, 7.5).Result);

            // Test paging
            TestHelpers.SearchPages(i => Config.Client.GetGuestSessionRatedTvAsync(i).Result);

            // Fetch ratings
            SearchContainer<SearchTvShowWithRating> result = Config.Client.GetGuestSessionRatedTvAsync().Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
        }

        [Fact]
        public void TestGuestSessionGetRatedMovies()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.True(Config.Client.MovieSetRatingAsync(IdHelper.Terminator, 7.5).Result);

            // Test paging
            TestHelpers.SearchPages(i => Config.Client.GetGuestSessionRatedMoviesAsync(i).Result);

            // Fetch ratings
            SearchContainer<SearchMovieWithRating> result = Config.Client.GetGuestSessionRatedMoviesAsync().Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
        }
    }
}