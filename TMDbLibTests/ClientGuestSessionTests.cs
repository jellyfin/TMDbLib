using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        [Fact]
        public async Task TestTvEpisodeSetRatingGuestSessionAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 7.5));

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            SearchContainer<TvEpisodeWithRating> ratings = await TMDbClient.GetGuestSessionRatedTvEpisodesAsync();

            Assert.False(true, "This test has been failing for some time - TMDb has been made aware, but have not responded");

            //double tmpRating = ratings.Results.Single(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1).Rating;
            //Assert.True(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
            //Assert.True(Math.Abs(7.5 - tmpRating) < float.Epsilon);

            //// Try changing it back to the previous rating
            //Assert.True(Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 8));

            //// Allow TMDb to cache our changes
            //await Task.Delay(2000);

            //ratings = Config.Client.GetGuestSessionRatedTvEpisodesAsync().Sync();

            //tmpRating = ratings.Results.Single(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1).Rating;
            //Assert.True(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
            //Assert.True(Math.Abs(8 - tmpRating) < float.Epsilon);

            //// Try removing the rating
            //Assert.True(Config.Client.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1));

            //// Allow TMDb to cache our changes
            //await Task.Delay(2000);

            //ratings = Config.Client.GetGuestSessionRatedTvEpisodesAsync().Sync();

            //Assert.False(ratings.Results.Any(s => s.ShowId == IdHelper.BreakingBad && s.SeasonNumber == 1 && s.EpisodeNumber == 1));
        }

        [Fact]
        public async Task TestTvSetRatingGuestSessionAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.House, 7.5));

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            SearchContainer<SearchTvShowWithRating> ratings = await TMDbClient.GetGuestSessionRatedTvAsync();

            double tmpRating = ratings.Results.Single(s => s.Id == IdHelper.House).Rating;
            Assert.Contains(ratings.Results, s => s.Id == IdHelper.House);
            Assert.True(Math.Abs(7.5 - tmpRating) < float.Epsilon);

            // Try changing it back to the previous rating
            Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.House, 8));

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            ratings = await TMDbClient.GetGuestSessionRatedTvAsync();

            tmpRating = ratings.Results.Single(s => s.Id == IdHelper.House).Rating;
            Assert.Contains(ratings.Results, s => s.Id == IdHelper.House);
            Assert.True(Math.Abs(8 - tmpRating) < float.Epsilon);

            // Try removing the rating
            Assert.True(await TMDbClient.TvShowRemoveRatingAsync(IdHelper.House));

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            ratings = await TMDbClient.GetGuestSessionRatedTvAsync();

            Assert.DoesNotContain(ratings.Results, s => s.Id == IdHelper.House);
        }

        [Fact]
        public async Task TestMoviesSetRatingGuestSessionAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(await TMDbClient.MovieSetRatingAsync(IdHelper.Terminator, 7.5));

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            SearchContainer<SearchMovieWithRating> ratings = await TMDbClient.GetGuestSessionRatedMoviesAsync();

            double tmpRating = ratings.Results.Single(s => s.Id == IdHelper.Terminator).Rating;
            Assert.Contains(ratings.Results, s => s.Id == IdHelper.Terminator);
            Assert.True(Math.Abs(7.5 - tmpRating) < float.Epsilon);

            // Try changing it back to the previous rating
            Assert.True(await TMDbClient.MovieSetRatingAsync(IdHelper.Terminator, 8));

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            ratings = await TMDbClient.GetGuestSessionRatedMoviesAsync();

            tmpRating = ratings.Results.Single(s => s.Id == IdHelper.Terminator).Rating;
            Assert.Contains(ratings.Results, s => s.Id == IdHelper.Terminator);
            Assert.True(Math.Abs(8 - tmpRating) < float.Epsilon);

            // Try removing the rating
            Assert.True(await TMDbClient.MovieRemoveRatingAsync(IdHelper.Terminator));

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            ratings = await TMDbClient.GetGuestSessionRatedMoviesAsync();

            Assert.DoesNotContain(ratings.Results, s => s.Id == IdHelper.Terminator);
        }

        [Fact]
        public async Task TestGuestSessionGetRatedTvEpisodesAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.True(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BigBangTheory, 1, 1, 7.5));

            // Test paging
            await TestHelpers.SearchPagesAsync(i => TMDbClient.GetGuestSessionRatedTvEpisodesAsync(i));

            // Fetch ratings
            SearchContainer<TvEpisodeWithRating> result = await TMDbClient.GetGuestSessionRatedTvEpisodesAsync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
        }

        [Fact]
        public async Task TestGuestSessionGetRatedTvAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.BigBangTheory, 7.5));

            // Test paging
            await TestHelpers.SearchPagesAsync(i => TMDbClient.GetGuestSessionRatedTvAsync(i));

            // Fetch ratings
            SearchContainer<SearchTvShowWithRating> result = await TMDbClient.GetGuestSessionRatedTvAsync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
        }

        [Fact]
        public async Task TestGuestSessionGetRatedMoviesAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

            // Ensure we have a rating
            Assert.True(await TMDbClient.MovieSetRatingAsync(IdHelper.Terminator, 7.5));

            // Test paging
            await TestHelpers.SearchPagesAsync(i => TMDbClient.GetGuestSessionRatedMoviesAsync(i));

            // Fetch ratings
            SearchContainer<SearchMovieWithRating> result = await TMDbClient.GetGuestSessionRatedMoviesAsync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
        }
    }
}