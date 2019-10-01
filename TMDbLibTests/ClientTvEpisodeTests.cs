using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public void TestTvEpisodeExtrasNone()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / account_states", " / credits", " / external_ids", " / images", " / videos");

            TvEpisode tvEpisode = Config.Client.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1).Result;

            TestBreakingBadSeasonOneEpisodeOneBaseProperties(tvEpisode);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvEpisode, object> selector in _methods.Values)
            {
                Assert.Null(selector(tvEpisode));
            }
        }

        [Fact]
        public void TestTvEpisodeExtrasAccountState()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / credits", " / external_ids", " / images", " / videos");

            // Test the custom parsing code for Account State rating
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            TvEpisode episode = Config.Client.GetTvEpisodeAsync(IdHelper.BigBangTheory, 1, 1, TvEpisodeMethods.AccountStates).Result;
            if (episode.AccountStates == null || !episode.AccountStates.Rating.HasValue)
            {
                Config.Client.TvEpisodeSetRatingAsync(IdHelper.BigBangTheory, 1, 1, 5).Sync();

                // Allow TMDb to update cache
                Thread.Sleep(2000);

                episode = Config.Client.GetTvEpisodeAsync(IdHelper.BigBangTheory, 1, 1, TvEpisodeMethods.AccountStates).Result;
            }

            Assert.NotNull(episode.AccountStates);
            Assert.True(episode.AccountStates.Rating.HasValue);
            Assert.True(Math.Abs(episode.AccountStates.Rating.Value - 5) < double.Epsilon);
        }

        [Fact]
        public void TestTvEpisodeExtrasAll()
        {
            IgnoreMissingJson("credits / id", "external_ids / id", "images / id", "videos / id");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 5).Sync();

            TvEpisodeMethods combinedEnum = _methods.Keys.Aggregate((methods, tvEpisodeMethods) => methods | tvEpisodeMethods);
            TvEpisode tvEpisode = Config.Client.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1, combinedEnum).Result;

            TestBreakingBadSeasonOneEpisodeOneBaseProperties(tvEpisode);

            Assert.NotNull(tvEpisode.Images);
            Assert.NotNull(tvEpisode.Images.Stills);
            Assert.True(tvEpisode.Images.Stills.Count > 0);

            TestMethodsHelper.TestAllNotNull(_methods, tvEpisode);
        }

        [Fact]
        public void TestTvEpisodeExtrasExclusive()
        {
            IgnoreMissingJson(" / account_states", " / credits", " / external_ids", " / images", " / videos", "credits / id", "external_ids / id", "images / id", "videos / id");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => Config.Client.GetTvEpisodeAsync(id, 1, 1, extras).Result, IdHelper.BreakingBad);
        }

        [Fact]
        public void TestTvEpisodeSeparateExtrasCredits()
        {
            CreditsWithGuestStars credits = Config.Client.GetTvEpisodeCreditsAsync(IdHelper.BreakingBad, 1, 1).Result;
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
        public void TestTvEpisodeSeparateExtrasExternalIds()
        {
            ExternalIdsTvEpisode externalIds = Config.Client.GetTvEpisodeExternalIdsAsync(IdHelper.BreakingBad, 1, 1).Result;

            Assert.NotNull(externalIds);
            Assert.True(string.IsNullOrEmpty(externalIds.FreebaseId));
            Assert.Equal(62085, externalIds.Id);
            Assert.Equal("/m/03mb620", externalIds.FreebaseMid);
            Assert.Equal("tt0959621", externalIds.ImdbId);
            Assert.Equal("637041", externalIds.TvrageId);
            Assert.Equal("349232", externalIds.TvdbId);
        }

        [Fact]
        public void TestTvEpisodeSeparateExtrasImages()
        {
            StillImages images = Config.Client.GetTvEpisodeImagesAsync(IdHelper.BreakingBad, 1, 1).Result;
            Assert.NotNull(images);
            Assert.NotNull(images.Stills);
        }

        [Fact]
        public void TestTvEpisodeSeparateExtrasVideos()
        {
            ResultContainer<Video> images = Config.Client.GetTvEpisodeVideosAsync(IdHelper.BreakingBad, 1, 1).Result;
            Assert.NotNull(images);
            Assert.NotNull(images.Results);
        }

        [Fact]
        public void TestTvEpisodeAccountStateRatingSet()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TvEpisodeAccountState accountState = Config.Client.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1).Result;

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(Config.Client.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1).Result);

                // Allow TMDb to cache our changes
                Thread.Sleep(2000);
            }

            // Test that the episode is NOT rated
            accountState = Config.Client.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1).Result;

            Assert.Equal(IdHelper.BreakingBadSeason1Episode1Id, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the episode
            Assert.True(Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 5).Result);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the episode IS rated
            accountState = Config.Client.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1).Result;
            Assert.Equal(IdHelper.BreakingBadSeason1Episode1Id, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(Config.Client.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1).Result);
        }

        [Fact]
        public void TestTvEpisodeRateBad()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            Assert.False(Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, -1).Result);
            Assert.False(Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 0).Result);
            Assert.False(Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 10.5).Result);
        }

        [Fact]
        public void TestTvEpisodeGetChanges()
        {
            ChangesContainer changes = Config.Client.GetTvEpisodeChangesAsync(IdHelper.BreakingBadSeason1Episode1Id).Result;

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
        public void TestTvEpisodeMissing()
        {
            TvEpisode tvEpisode = Config.Client.GetTvEpisodeAsync(IdHelper.MissingID, 1, 1).Result;

            Assert.Null(tvEpisode);
        }

        [Fact]
        public void TestTvEpisodesScreenedTheatrically()
        {
            var results = Config.Client.GetTvEpisodesScreenedTheatricallyAsync(IdHelper.GameOfThrones).Result;

            Assert.Equal(IdHelper.GameOfThrones, results.Id);
            Assert.NotNull(results);
            Assert.NotNull(results.Results);
            Assert.True(results.Results.Count > 0);
        }
    }
}