using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Cast = TMDbLib.Objects.TvShows.Cast;

namespace TMDbLibTests
{
    public class ClientTvEpisodeTests : TestBase
    {
        private static readonly Dictionary<TvEpisodeMethods, Func<TvEpisode, object>> Methods;

        static ClientTvEpisodeTests()
        {
            Methods = new Dictionary<TvEpisodeMethods, Func<TvEpisode, object>>
            {
                [TvEpisodeMethods.Credits] = tvEpisode => tvEpisode.Credits,
                [TvEpisodeMethods.Images] = tvEpisode => tvEpisode.Images,
                [TvEpisodeMethods.ExternalIds] = tvEpisode => tvEpisode.ExternalIds,
                [TvEpisodeMethods.Videos] = tvEpisode => tvEpisode.Videos,
                [TvEpisodeMethods.AccountStates] = tvEpisode => tvEpisode.AccountStates
            };
        }

        [Fact]
        public async Task TestTvEpisodeExtrasNoneAsync()
        {
            TvEpisode tvEpisode = await TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1);

            await Verify(tvEpisode);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvEpisode, object> selector in Methods.Values)
            {
                Assert.Null(selector(tvEpisode));
            }
        }

        [Fact]
        public async Task TestTvEpisodeExtrasAccountState()
        {
            // Test the custom parsing code for Account State rating
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            TvEpisode episode = await TMDbClient.GetTvEpisodeAsync(IdHelper.BigBangTheory, 1, 1, TvEpisodeMethods.AccountStates);
            if (episode.AccountStates == null || !episode.AccountStates.Rating.HasValue)
            {
                await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BigBangTheory, 1, 1, 5);

                // Allow TMDb to update cache
                await Task.Delay(2000);

                episode = await TMDbClient.GetTvEpisodeAsync(IdHelper.BigBangTheory, 1, 1, TvEpisodeMethods.AccountStates);
            }

            await Verify(episode);
        }

        [Fact]
        public async Task TestTvEpisodeExtrasAll()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.FullerHouse, 1, 1, 5);

            await TestMethodsHelper.TestGetAll(Methods, combined => TMDbClient.GetTvEpisodeAsync(IdHelper.FullerHouse, 1, 1, combined), async tvEpisode =>
                {
                    await Verify(tvEpisode);
                });
        }

        [Fact]
        public async Task TestTvEpisodeExtrasExclusiveAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            await TestMethodsHelper.TestGetExclusive(Methods, extras => TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1, extras));
        }

        [Fact]
        public async Task TestTvEpisodeSeparateExtrasCreditsAsync()
        {
            CreditsWithGuestStars credits = await TMDbClient.GetTvEpisodeCreditsAsync(IdHelper.BreakingBad, 1, 1);
            Assert.NotNull(credits);

            Cast guestStarItem = credits.GuestStars.FirstOrDefault(s => s.Id == 92495);
            Cast castItem = credits.Cast.FirstOrDefault(s => s.Id == 17419);
            Crew crewItem = credits.Crew.FirstOrDefault(s => s.Id == 1280071);

            await Verify(new
            {
                guestStarItem,
                castItem,
                crewItem
            });
        }

        [Fact]
        public async Task TestTvEpisodeSeparateExtrasExternalIdsAsync()
        {
            ExternalIdsTvEpisode externalIds = await TMDbClient.GetTvEpisodeExternalIdsAsync(IdHelper.BreakingBad, 1, 1);

            await Verify(externalIds);
        }

        [Fact]
        public async Task TestTvEpisodeSeparateExtrasImagesAsync()
        {
            StillImages images = await TMDbClient.GetTvEpisodeImagesAsync(IdHelper.BreakingBad, 1, 1);

            await Verify(images);
        }

        [Fact]
        public async Task TestTvEpisodeSeparateExtrasVideosAsync()
        {
            ResultContainer<Video> images = await TMDbClient.GetTvEpisodeVideosAsync(IdHelper.BreakingBad, 1, 1);

            await Verify(images);
        }

        [Fact]
        public async Task TestTvEpisodeAccountStateRatingSetAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            TvEpisodeAccountState accountState = await TMDbClient.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1);

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(await TMDbClient.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1));

                // Allow TMDb to cache our changes
                await Task.Delay(2000);
            }

            // Test that the episode is NOT rated
            accountState = await TMDbClient.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1);

            Assert.Equal(IdHelper.BreakingBadSeason1Episode1Id, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the episode
            Assert.True(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 5));

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the episode IS rated
            accountState = await TMDbClient.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1);
            Assert.Equal(IdHelper.BreakingBadSeason1Episode1Id, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(await TMDbClient.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1));
        }

        [Fact]
        public async Task TestTvEpisodeRateBadAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            Assert.False(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, -1));
            Assert.False(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 0));
            Assert.False(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 10.5));
        }

        [Fact]
        public async Task TestTvEpisodeGetChangesAsync()
        {
            IList<Change> changes = await TMDbClient.GetTvEpisodeChangesAsync(IdHelper.BreakingBadSeason1Episode1Id);

            Assert.NotEmpty(changes);

            await Verify(changes);
        }

        [Fact]
        public async Task TestTvEpisodeMissingAsync()
        {
            TvEpisode tvEpisode = await TMDbClient.GetTvEpisodeAsync(IdHelper.MissingID, 1, 1);

            Assert.Null(tvEpisode);
        }

        [Fact]
        public async Task TestTvEpisodesScreenedTheatricallyAsync()
        {
            ResultContainer<TvEpisodeInfo> results = await TMDbClient.GetTvEpisodesScreenedTheatricallyAsync(IdHelper.GameOfThrones);
            TvEpisodeInfo single = results.Results.Single(s => s.Id == IdHelper.GameOfThronesSeason4Episode10);

            await Verify(single);
        }

        [Fact]
        public async Task TestTvEpisodeGetTvEpisodeWithImageLanguageAsync()
        {
            TvEpisode resp = await TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1, language: "en-US", includeImageLanguage: "en", extraMethods: TvEpisodeMethods.Images);

            await Verify(resp.Images);
        }
    }
}
