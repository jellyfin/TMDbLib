using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientTvSeasonTests
    {
        private const int BreakingBad = 1396;
        private const int BreakingBadSeason1Id = 3572;
        private const int BigBangTheory = 1418;

        private static Dictionary<TvSeasonMethods, Func<TvSeason, object>> _methods;
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        /// <summary>
        /// Run once, on test class initialization
        /// </summary>
        [ClassInitialize]
        public static void InitialInitiator(TestContext context)
        {
            _methods = new Dictionary<TvSeasonMethods, Func<TvSeason, object>>();
            _methods[TvSeasonMethods.Credits] = tvSeason => tvSeason.Credits;
            _methods[TvSeasonMethods.Images] = tvSeason => tvSeason.Images;
            _methods[TvSeasonMethods.ExternalIds] = tvSeason => tvSeason.ExternalIds;
            _methods[TvSeasonMethods.Videos] = tvSeason => tvSeason.Videos;
            _methods[TvSeasonMethods.Videos] = tvSeason => tvSeason.Videos;
            _methods[TvSeasonMethods.AccountStates] = tvSeason => tvSeason.AccountStates;
        }

        [TestMethod]
        public void TestTvSeasonExtrasNone()
        {
            TvSeason tvSeason = _config.Client.GetTvSeason(BreakingBad, 1);

            TestBreakingBadBaseProperties(tvSeason);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvSeason, object> selector in _methods.Values)
            {
                Assert.IsNull(selector(tvSeason));
            }
        }

        [TestMethod]
        public void TestTvSeasonExtrasAccountState()
        {
            // Test the custom parsing code for Account State rating
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);

            TvSeason season = _config.Client.GetTvSeason(BigBangTheory, 1, TvSeasonMethods.AccountStates);
            if (season.AccountStates == null || season.AccountStates.Results.All(s => s.EpisodeNumber != 1))
            {
                _config.Client.TvEpisodeSetRating(BigBangTheory, 1, 1, 5);

                // Allow TMDb to update cache
                Thread.Sleep(2000);

                season = _config.Client.GetTvSeason(BigBangTheory, 1, TvSeasonMethods.AccountStates);
            }

            Assert.IsNotNull(season.AccountStates);
            Assert.IsTrue(season.AccountStates.Results.Single(s => s.EpisodeNumber == 1).Rating.HasValue);
            Assert.IsTrue(Math.Abs(season.AccountStates.Results.Single(s => s.EpisodeNumber == 1).Rating.Value - 5) < double.Epsilon);
        }

        [TestMethod]
        public void TestTvSeasonExtrasAll()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            _config.Client.TvEpisodeSetRating(BreakingBad, 1, 1, 5);

            TvSeasonMethods combinedEnum = _methods.Keys.Aggregate((methods, tvSeasonMethods) => methods | tvSeasonMethods);
            TvSeason tvSeason = _config.Client.GetTvSeason(BreakingBad, 1, combinedEnum);

            TestBreakingBadBaseProperties(tvSeason);

            TestMethodsHelper.TestAllNotNull(_methods, tvSeason);
        }

        [TestMethod]
        public void TestTvSeasonExtrasExclusive()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => _config.Client.GetTvSeason(id, 1, extras), BreakingBad);
        }

        [TestMethod]
        public void TestTvSeasonSeparateExtrasCredits()
        {
            Credits credits = _config.Client.GetTvSeasonCredits(BreakingBad, 1);
            Assert.IsNotNull(credits);
            Assert.IsNotNull(credits.Cast);
            Assert.AreEqual("Walter White", credits.Cast[0].Character);
            Assert.AreEqual("52542282760ee313280017f9", credits.Cast[0].CreditId);
            Assert.AreEqual(17419, credits.Cast[0].Id);
            Assert.AreEqual("Bryan Cranston", credits.Cast[0].Name);
            Assert.IsNotNull(credits.Cast[0].ProfilePath);
            Assert.AreEqual(0, credits.Cast[0].Order);

            Crew crewPersonId = credits.Crew.FirstOrDefault(s => s.Id == 1223202);
            Assert.IsNotNull(crewPersonId);

            Assert.AreEqual(1223202, crewPersonId.Id);
            Assert.AreEqual("Production", crewPersonId.Department);
            Assert.AreEqual("Diane Mercer", crewPersonId.Name);
            Assert.AreEqual("Producer", crewPersonId.Job);
            Assert.IsNull(crewPersonId.ProfilePath);
        }

        [TestMethod]
        public void TestTvSeasonSeparateExtrasExternalIds()
        {
            ExternalIds externalIds = _config.Client.GetTvSeasonExternalIds(BreakingBad, 1);
            Assert.IsNotNull(externalIds);
            Assert.AreEqual(3572, externalIds.Id);
            Assert.AreEqual("/en/breaking_bad_season_1", externalIds.FreebaseId);
            Assert.AreEqual("/m/05yy27m", externalIds.FreebaseMid);
            Assert.IsNull(externalIds.ImdbId);
            Assert.IsNull(externalIds.TvrageId);
            Assert.AreEqual(30272, externalIds.TvdbId);
        }

        [TestMethod]
        public void TestTvSeasonSeparateExtrasImages()
        {
            PosterImages images = _config.Client.GetTvSeasonImages(BreakingBad, 1);
            Assert.IsNotNull(images);
            Assert.IsNotNull(images.Posters);
        }

        [TestMethod]
        public void TestTvSeasonSeparateExtrasVideos()
        {
            ResultContainer<Video> videos = _config.Client.GetTvSeasonVideos(BreakingBad, 1);
            Assert.IsNotNull(videos);
            Assert.IsNotNull(videos.Results);
        }

        [TestMethod]
        public void TestTvSeasonEpisodeCount()
        {
            TvSeason season = _config.Client.GetTvSeason(BreakingBad, 1);
            Assert.IsNotNull(season);
            Assert.IsNotNull(season.Episodes);

            Assert.AreEqual(season.Episodes.Count, season.EpisodeCount);
        }

        [TestMethod]
        public void TestTvSeasonAccountStateRatingSet()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);

            // Rate episode 1, 2 and 3 of BreakingBad
            Assert.IsTrue(_config.Client.TvEpisodeSetRating(BreakingBad, 1, 1, 5));
            Assert.IsTrue(_config.Client.TvEpisodeSetRating(BreakingBad, 1, 2, 7));
            Assert.IsTrue(_config.Client.TvEpisodeSetRating(BreakingBad, 1, 3, 3));

            // Wait for TMDb to un-cache our value
            Thread.Sleep(2000);

            // Fetch out the seasons state
            ResultContainer<TvEpisodeAccountState> state = _config.Client.GetTvSeasonAccountState(BreakingBad, 1);
            Assert.IsNotNull(state);

            Assert.IsTrue(Math.Abs(5 - (state.Results.Single(s => s.EpisodeNumber == 1).Rating ?? 0)) < double.Epsilon);
            Assert.IsTrue(Math.Abs(7 - (state.Results.Single(s => s.EpisodeNumber == 2).Rating ?? 0)) < double.Epsilon);
            Assert.IsTrue(Math.Abs(3 - (state.Results.Single(s => s.EpisodeNumber == 3).Rating ?? 0)) < double.Epsilon);

            // Test deleting Ratings
            Assert.IsTrue(_config.Client.TvEpisodeRemoveRating(BreakingBad, 1, 1));
            Assert.IsTrue(_config.Client.TvEpisodeRemoveRating(BreakingBad, 1, 2));
            Assert.IsTrue(_config.Client.TvEpisodeRemoveRating(BreakingBad, 1, 3));

            // Wait for TMDb to un-cache our value
            Thread.Sleep(2000);

            state = _config.Client.GetTvSeasonAccountState(BreakingBad, 1);
            Assert.IsNotNull(state);

            Assert.IsNull(state.Results.Single(s => s.EpisodeNumber == 1).Rating);
            Assert.IsNull(state.Results.Single(s => s.EpisodeNumber == 2).Rating);
            Assert.IsNull(state.Results.Single(s => s.EpisodeNumber == 3).Rating);
        }

        [TestMethod]
        public void TestTvSeasonGetChanges()
        {
            ChangesContainer changes = _config.Client.GetTvSeasonChanges(BreakingBadSeason1Id);
            Assert.IsNotNull(changes);
            Assert.IsNotNull(changes.Changes);
        }

        private void TestBreakingBadBaseProperties(TvSeason tvSeason)
        {
            Assert.IsNotNull(tvSeason);
            Assert.IsNotNull(tvSeason.Id);
            Assert.AreEqual(1, tvSeason.SeasonNumber);
            Assert.AreEqual("Season 1", tvSeason.Name);
            Assert.IsNotNull(tvSeason.AirDate);
            Assert.IsNotNull(tvSeason.Overview);
            Assert.IsNotNull(tvSeason.PosterPath);

            Assert.IsNotNull(tvSeason.Episodes);
            Assert.AreEqual(7, tvSeason.Episodes.Count);
            Assert.IsNotNull(tvSeason.Episodes[0].Id);
            Assert.AreEqual(1, tvSeason.Episodes[0].EpisodeNumber);
            Assert.AreEqual("Pilot", tvSeason.Episodes[0].Name);
            Assert.IsNotNull(tvSeason.Episodes[0].Overview);
            Assert.IsNull(tvSeason.Episodes[0].ProductionCode);
            Assert.AreEqual(1, tvSeason.Episodes[0].SeasonNumber);
            Assert.IsNotNull(tvSeason.Episodes[0].StillPath);
        }

        //[TestMethod]
        //public void TestMoviesLanguage()
        //{
        //    Movie movie = _config.Client.GetMovie(AGoodDayToDieHard);
        //    Movie movieItalian = _config.Client.GetMovie(AGoodDayToDieHard, "it");

        //    Assert.IsNotNull(movie);
        //    Assert.IsNotNull(movieItalian);

        //    Assert.AreEqual("A Good Day to Die Hard", movie.Title);
        //    Assert.AreNotEqual(movie.Title, movieItalian.Title);

        //    // Test all extras, ensure none of them exist
        //    foreach (Func<Movie, object> selector in _methods.Values)
        //    {
        //        Assert.IsNull(selector(movie));
        //        Assert.IsNull(selector(movieItalian));
        //    }
        //}
    }
}
