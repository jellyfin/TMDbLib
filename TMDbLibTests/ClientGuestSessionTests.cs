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

            await TestMethodsHelper.SetValidateRemoveTest(async () =>
            {
                Assert.True(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 7.5));
            }, async () =>
            {
                Assert.True(await TMDbClient.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1));
            }, async shouldBeSet =>
            {
                SearchContainer<TvEpisodeWithRating> ratings = await TMDbClient.GetGuestSessionRatedTvEpisodesAsync();

                if (shouldBeSet)
                {
                    Assert.Contains(ratings.Results, x => x.ShowId == IdHelper.BreakingBad && x.SeasonNumber == 1 && x.EpisodeNumber == 1);
                }
                else
                {
                    Assert.DoesNotContain(ratings.Results, x => x.ShowId == IdHelper.BreakingBad && x.SeasonNumber == 1 && x.EpisodeNumber == 1);
                }
            });
        }

        [Fact]
        public async Task TestTvSetRatingGuestSessionAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

            await TestMethodsHelper.SetValidateRemoveTest(async () =>
            {
                Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.House, 7.5));
            }, async () =>
            {
                Assert.True(await TMDbClient.TvShowRemoveRatingAsync(IdHelper.House));
            }, async shouldBeSet =>
            {
                SearchContainer<SearchTvShowWithRating> ratings = await TMDbClient.GetGuestSessionRatedTvAsync();

                if (shouldBeSet)
                {
                    Assert.Contains(ratings.Results, x => x.Id == IdHelper.House);
                }
                else
                {
                    Assert.DoesNotContain(ratings.Results, x => x.Id == IdHelper.House);
                }
            });
        }

        [Fact]
        public async Task TestMoviesSetRatingGuestSessionAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

            await TestMethodsHelper.SetValidateRemoveTest(async () =>
            {
                Assert.True(await TMDbClient.MovieSetRatingAsync(IdHelper.Terminator, 7.5));
            }, async () =>
            {
                Assert.True(await TMDbClient.MovieRemoveRatingAsync(IdHelper.Terminator));
            }, async shouldBeSet =>
            {
                SearchContainer<SearchMovieWithRating> ratings = await TMDbClient.GetGuestSessionRatedMoviesAsync();

                if (shouldBeSet)
                {
                    Assert.Contains(ratings.Results, x => x.Id == IdHelper.Terminator);
                }
                else
                {
                    Assert.DoesNotContain(ratings.Results, x => x.Id == IdHelper.Terminator);
                }
            });
        }
    }
}
