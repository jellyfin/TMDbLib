using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private static Dictionary<TvEpisodeMethods, Func<TvEpisode, object>> _methods;

        public ClientTvEpisodeTests()
        {
            _methods = new Dictionary<TvEpisodeMethods, Func<TvEpisode, object>>
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

            TestBreakingBadSeasonOneEpisodeOneBaseProperties(tvEpisode);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvEpisode, object> selector in _methods.Values)
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

            Assert.NotNull(episode.AccountStates);
            Assert.True(episode.AccountStates.Rating.HasValue);
            Assert.True(Math.Abs(episode.AccountStates.Rating.Value - 5) < double.Epsilon);
        }

        [Fact]
        public async Task TestTvEpisodeExtrasAll()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 5);

            await TestMethodsHelper.TestGetAll(_methods, combined => TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1, combined),
                tvEpisode =>
                {
                    TestBreakingBadSeasonOneEpisodeOneBaseProperties(tvEpisode);

                    Assert.NotNull(tvEpisode.Images);
                    Assert.NotNull(tvEpisode.Images.Stills);
                    Assert.True(tvEpisode.Images.Stills.Count > 0);
                });
        }

        [Fact]
        public async Task TestTvEpisodeExtrasExclusiveAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            await TestMethodsHelper.TestGetExclusive(_methods, extras => TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1, extras));
        }

        [Fact]
        public async Task TestTvEpisodeSeparateExtrasCreditsAsync()
        {
            CreditsWithGuestStars credits = await TMDbClient.GetTvEpisodeCreditsAsync(IdHelper.BreakingBad, 1, 1);
            Assert.NotNull(credits);

            Cast guestStarItem = credits.GuestStars.FirstOrDefault(s => s.Id == 92495);
            Assert.Equal(92495, guestStarItem.Id);
            Assert.Equal("Emilio Koyama", guestStarItem.Character);
            Assert.Equal("52542273760ee3132800068e", guestStarItem.CreditId);
            Assert.Equal("John Koyama", guestStarItem.Name);
            Assert.NotNull(guestStarItem.ProfilePath);
            Assert.Equal(1, guestStarItem.Order);

            Cast castItem = credits.Cast.FirstOrDefault(s => s.Id == 17419);
            Assert.Equal(17419, castItem.Id);
            Assert.Equal("Walter White", castItem.Character);
            Assert.Equal("52542282760ee313280017f9", castItem.CreditId);
            Assert.Equal("Bryan Cranston", castItem.Name);
            Assert.NotNull(castItem.ProfilePath);
            Assert.Equal(0, castItem.Order);

            Crew crewItem = credits.Crew.FirstOrDefault(s => s.Id == 1280071);
            Assert.NotNull(crewItem);
            Assert.Equal(1280071, crewItem.Id);
            Assert.Equal("Editing", crewItem.Department);
            Assert.Equal("Lynne Willingham", crewItem.Name);
            Assert.Equal("Editor", crewItem.Job);
            Assert.Null(crewItem.ProfilePath);
        }

        [Fact]
        public async Task TestTvEpisodeSeparateExtrasExternalIdsAsync()
        {
            ExternalIdsTvEpisode externalIds = await TMDbClient.GetTvEpisodeExternalIdsAsync(IdHelper.BreakingBad, 1, 1);

            Assert.NotNull(externalIds);
            Assert.True(string.IsNullOrEmpty(externalIds.FreebaseId));
            Assert.Equal(62085, externalIds.Id);
            Assert.Equal("/m/03mb620", externalIds.FreebaseMid);
            Assert.Equal("tt0959621", externalIds.ImdbId);
            Assert.Equal("637041", externalIds.TvrageId);
            Assert.Equal("349232", externalIds.TvdbId);
        }

        [Fact]
        public async Task TestTvEpisodeSeparateExtrasImagesAsync()
        {
            StillImages images = await TMDbClient.GetTvEpisodeImagesAsync(IdHelper.BreakingBad, 1, 1);
            Assert.NotNull(images);
            Assert.NotNull(images.Stills);
        }

        [Fact]
        public async Task TestTvEpisodeSeparateExtrasVideosAsync()
        {
            ResultContainer<Video> images = await TMDbClient.GetTvEpisodeVideosAsync(IdHelper.BreakingBad, 1, 1);
            Assert.NotNull(images);
            Assert.NotNull(images.Results);
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
            ChangesContainer changes = await TMDbClient.GetTvEpisodeChangesAsync(IdHelper.BreakingBadSeason1Episode1Id);

            Assert.NotNull(changes);
            Assert.NotNull(changes.Changes);
        }

        private void TestBreakingBadSeasonOneEpisodeOneBaseProperties(TvEpisode tvEpisode)
        {
            Assert.Equal(62085, tvEpisode.Id);
            Assert.True(tvEpisode.AirDate.HasValue);
            Assert.Equal(new DateTime(2008, 1, 19), tvEpisode.AirDate.Value.Date);
            Assert.Equal(1, tvEpisode.EpisodeNumber);
            Assert.Equal("Pilot", tvEpisode.Name);
            Assert.NotNull(tvEpisode.Overview);
            Assert.Null(tvEpisode.ProductionCode);
            Assert.Equal(1, tvEpisode.SeasonNumber);
            Assert.NotNull(tvEpisode.StillPath);

            Assert.NotNull(tvEpisode.Crew);
            Crew crew = tvEpisode.Crew.SingleOrDefault(s => s.CreditId == "52542275760ee313280006ce");
            Assert.NotNull(crew);

            Assert.Equal(66633, crew.Id);
            Assert.Equal("52542275760ee313280006ce", crew.CreditId);
            Assert.Equal("Vince Gilligan", crew.Name);
            Assert.Equal("Writing", crew.Department);
            Assert.Equal("Writer", crew.Job);
            Assert.True(TestImagesHelpers.TestImagePath(crew.ProfilePath), "crew.ProfilePath was not a valid image path, was: " + crew.ProfilePath);

            Assert.NotNull(tvEpisode.GuestStars);
            Cast star = tvEpisode.GuestStars.SingleOrDefault(s => s.CreditId == "52542273760ee3132800068e");
            Assert.NotNull(star);

            Assert.Equal(92495, star.Id);
            Assert.Equal("John Koyama", star.Name);
            Assert.Equal("52542273760ee3132800068e", star.CreditId);
            Assert.Equal("Emilio Koyama", star.Character);
            Assert.Equal(1, star.Order);
            Assert.True(TestImagesHelpers.TestImagePath(star.ProfilePath), "star.ProfilePath was not a valid image path, was: " + star.ProfilePath);
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

            Assert.Equal(IdHelper.GameOfThrones, results.Id);

            TvEpisodeInfo single = results.Results.FirstOrDefault(s => s.Id == 63103);
            Assert.NotNull(single);
            Assert.Equal(4, single.SeasonNumber);
            Assert.Equal(10, single.EpisodeNumber);
        }

        [Fact]
        public async Task TestTvEpisodeGetTvEpisodeWithImageLanguageAsync()
        {
            TvEpisode resp = await TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1, language: "en-US", includeImageLanguage: "en", extraMethods: TvEpisodeMethods.Images);

            Assert.True(resp.Images.Stills.Count > 0);
        }
    }
}