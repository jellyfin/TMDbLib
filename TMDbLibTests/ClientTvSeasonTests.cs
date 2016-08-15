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
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLibTests
{
    public class ClientTvSeasonTests : TestBase
    {
        private static Dictionary<TvSeasonMethods, Func<TvSeason, object>> _methods;

        public ClientTvSeasonTests()
        {
            _methods = new Dictionary<TvSeasonMethods, Func<TvSeason, object>>
            {
                [TvSeasonMethods.Credits] = tvSeason => tvSeason.Credits,
                [TvSeasonMethods.Images] = tvSeason => tvSeason.Images,
                [TvSeasonMethods.ExternalIds] = tvSeason => tvSeason.ExternalIds,
                [TvSeasonMethods.Videos] = tvSeason => tvSeason.Videos,
                [TvSeasonMethods.Videos] = tvSeason => tvSeason.Videos,
                [TvSeasonMethods.AccountStates] = tvSeason => tvSeason.AccountStates
            };
        }

        [Fact]
        public void TestTvSeasonExtrasNone()
        {
            // TMDb is sending an extra property
            IgnoreMissingCSharp("_id / _id");

            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / images", " / account_states", " / credits", " / external_ids", " / images", " / videos");

            TvSeason tvSeason = Config.Client.GetTvSeasonAsync(IdHelper.BreakingBad, 1).Result;

            TestBreakingBadBaseProperties(tvSeason);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvSeason, object> selector in _methods.Values)
            {
                Assert.Null(selector(tvSeason));
            }
        }

        [Fact]
        public void TestTvSeasonExtrasAccountState()
        {
            // TMDb is sending an extra property
            IgnoreMissingCSharp("_id / _id");

            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / credits", " / external_ids", " / images", " / videos", "account_states / id");

            // Test the custom parsing code for Account State rating
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            TvSeason season = Config.Client.GetTvSeasonAsync(IdHelper.BigBangTheory, 1, TvSeasonMethods.AccountStates).Result;
            if (season.AccountStates == null || season.AccountStates.Results.All(s => s.EpisodeNumber != 1))
            {
                Config.Client.TvEpisodeSetRatingAsync(IdHelper.BigBangTheory, 1, 1, 5).Sync();

                // Allow TMDb to update cache
                Thread.Sleep(2000);

                season = Config.Client.GetTvSeasonAsync(IdHelper.BigBangTheory, 1, TvSeasonMethods.AccountStates).Result;
            }

            Assert.NotNull(season.AccountStates);
            Assert.True(season.AccountStates.Results.Single(s => s.EpisodeNumber == 1).Rating.HasValue);
            Assert.True(Math.Abs(season.AccountStates.Results.Single(s => s.EpisodeNumber == 1).Rating.Value - 5) < double.Epsilon);
        }

        [Fact]
        public void TestTvSeasonExtrasAll()
        {
            // TMDb is sending an extra property
            IgnoreMissingCSharp("_id / _id");

            IgnoreMissingJson("images / id", "account_states / id", "credits / id", "external_ids / id", "videos / id");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 5).Sync();

            TvSeasonMethods combinedEnum = _methods.Keys.Aggregate((methods, tvSeasonMethods) => methods | tvSeasonMethods);
            TvSeason tvSeason = Config.Client.GetTvSeasonAsync(IdHelper.BreakingBad, 1, combinedEnum).Result;

            TestBreakingBadBaseProperties(tvSeason);

            TestMethodsHelper.TestAllNotNull(_methods, tvSeason);
        }

        [Fact]
        public void TestTvSeasonExtrasExclusive()
        {
            // TMDb is sending an extra property
            IgnoreMissingCSharp("_id / _id");

            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / images", " / account_states", " / external_ids", " / images", " / videos", " / credits", "images / id", "external_ids / id", "videos / id", "credits / id", "account_states / id");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => Config.Client.GetTvSeasonAsync(id, 1, extras).Result, IdHelper.BreakingBad);
        }

        [Fact]
        public void TestTvSeasonSeparateExtrasCredits()
        {
            Credits credits = Config.Client.GetTvSeasonCreditsAsync(IdHelper.BreakingBad, 1).Result;
            Assert.NotNull(credits);
            Assert.NotNull(credits.Cast);
            Assert.Equal("Walter White", credits.Cast[0].Character);
            Assert.Equal("52542282760ee313280017f9", credits.Cast[0].CreditId);
            Assert.Equal(17419, credits.Cast[0].Id);
            Assert.Equal("Bryan Cranston", credits.Cast[0].Name);
            Assert.NotNull(credits.Cast[0].ProfilePath);
            Assert.Equal(0, credits.Cast[0].Order);

            Crew crewPersonId = credits.Crew.FirstOrDefault(s => s.Id == 1223202);
            Assert.NotNull(crewPersonId);

            Assert.Equal(1223202, crewPersonId.Id);
            Assert.Equal("Production", crewPersonId.Department);
            Assert.Equal("Diane Mercer", crewPersonId.Name);
            Assert.Equal("Producer", crewPersonId.Job);
            Assert.Null(crewPersonId.ProfilePath);
        }

        [Fact]
        public void TestTvSeasonSeparateExtrasExternalIds()
        {
            ExternalIdsTvSeason externalIds = Config.Client.GetTvSeasonExternalIdsAsync(IdHelper.BreakingBad, 1).Result;

            Assert.NotNull(externalIds);
            Assert.Equal(3572, externalIds.Id);
            Assert.Equal("/en/breaking_bad_season_1", externalIds.FreebaseId);
            Assert.Equal("/m/05yy27m", externalIds.FreebaseMid);
            Assert.Null(externalIds.TvrageId);
            Assert.Equal("30272", externalIds.TvdbId);
        }

        [Fact]
        public void TestTvSeasonSeparateExtrasImages()
        {
            PosterImages images = Config.Client.GetTvSeasonImagesAsync(IdHelper.BreakingBad, 1).Result;
            Assert.NotNull(images);
            Assert.NotNull(images.Posters);
        }

        [Fact]
        public void TestTvSeasonSeparateExtrasVideos()
        {
            ResultContainer<Video> videos = Config.Client.GetTvSeasonVideosAsync(IdHelper.BreakingBad, 1).Result;
            Assert.NotNull(videos);
            Assert.NotNull(videos.Results);
        }

        [Fact]
        public void TestTvSeasonAccountStateRatingSet()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Rate episode 1, 2 and 3 of BreakingBad
            Assert.True(Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 5).Result);
            Assert.True(Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 2, 7).Result);
            Assert.True(Config.Client.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 3, 3).Result);

            // Wait for TMDb to un-cache our value
            Thread.Sleep(2000);

            // Fetch out the seasons state
            ResultContainer<TvEpisodeAccountStateWithNumber> state = Config.Client.GetTvSeasonAccountStateAsync(IdHelper.BreakingBad, 1).Result;
            Assert.NotNull(state);

            Assert.True(Math.Abs(5 - (state.Results.Single(s => s.EpisodeNumber == 1).Rating ?? 0)) < double.Epsilon);
            Assert.True(Math.Abs(7 - (state.Results.Single(s => s.EpisodeNumber == 2).Rating ?? 0)) < double.Epsilon);
            Assert.True(Math.Abs(3 - (state.Results.Single(s => s.EpisodeNumber == 3).Rating ?? 0)) < double.Epsilon);

            // Test deleting Ratings
            Assert.True(Config.Client.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1).Result);
            Assert.True(Config.Client.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 2).Result);
            Assert.True(Config.Client.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 3).Result);

            // Wait for TMDb to un-cache our value
            Thread.Sleep(2000);

            state = Config.Client.GetTvSeasonAccountStateAsync(IdHelper.BreakingBad, 1).Result;
            Assert.NotNull(state);

            Assert.Null(state.Results.Single(s => s.EpisodeNumber == 1).Rating);
            Assert.Null(state.Results.Single(s => s.EpisodeNumber == 2).Rating);
            Assert.Null(state.Results.Single(s => s.EpisodeNumber == 3).Rating);
        }

        [Fact]
        public void TestTvSeasonGetChanges()
        {
            ChangesContainer changes = Config.Client.GetTvSeasonChangesAsync(IdHelper.BreakingBadSeason1Id).Result;
            Assert.NotNull(changes);
            Assert.NotNull(changes.Changes);
        }

        private void TestBreakingBadBaseProperties(TvSeason tvSeason)
        {
            Assert.NotNull(tvSeason);
            Assert.NotNull(tvSeason.Id);
            Assert.Equal(1, tvSeason.SeasonNumber);
            Assert.Equal("Season 1", tvSeason.Name);
            Assert.NotNull(tvSeason.AirDate);
            Assert.NotNull(tvSeason.Overview);
            Assert.NotNull(tvSeason.PosterPath);

            Assert.NotNull(tvSeason.Episodes);
            Assert.Equal(7, tvSeason.Episodes.Count);
            Assert.NotNull(tvSeason.Episodes[0].Id);
            Assert.Equal(1, tvSeason.Episodes[0].EpisodeNumber);
            Assert.Equal("Pilot", tvSeason.Episodes[0].Name);
            Assert.NotNull(tvSeason.Episodes[0].Overview);
            Assert.Null(tvSeason.Episodes[0].ProductionCode);
            Assert.Equal(1, tvSeason.Episodes[0].SeasonNumber);
            Assert.NotNull(tvSeason.Episodes[0].StillPath);
        }

        //[Fact]
        //public void TestMoviesLanguage()
        //{
        //    Movie movie = _config.Client.GetMovieAsync(AGoodDayToDieHard);
        //    Movie movieItalian = _config.Client.GetMovieAsync(AGoodDayToDieHard, "it");

        //    Assert.NotNull(movie);
        //    Assert.NotNull(movieItalian);

        //    Assert.Equal("A Good Day to Die Hard", movie.Title);
        //    Assert.NotEqual(movie.Title, movieItalian.Title);

        //    // Test all extras, ensure none of them exist
        //    foreach (Func<Movie, object> selector in _methods.Values)
        //    {
        //        Assert.Null(selector(movie));
        //        Assert.Null(selector(movieItalian));
        //    }
        //}
    }
}
