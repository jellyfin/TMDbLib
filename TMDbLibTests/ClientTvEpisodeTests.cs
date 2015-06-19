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
using Cast = TMDbLib.Objects.TvShows.Cast;
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientTvEpisodeTests
    {
        private const int BreakingBad = 1396;
        private const int BreakingBadSeason1Episode1Id = 62085;

        private static Dictionary<TvEpisodeMethods, Func<TvEpisode, object>> _methods;
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
            _methods = new Dictionary<TvEpisodeMethods, Func<TvEpisode, object>>();
            _methods[TvEpisodeMethods.Credits] = tvEpisode => tvEpisode.Credits;
            _methods[TvEpisodeMethods.Images] = tvEpisode => tvEpisode.Images;
            _methods[TvEpisodeMethods.ExternalIds] = tvEpisode => tvEpisode.ExternalIds;
            _methods[TvEpisodeMethods.Videos] = tvEpisode => tvEpisode.Videos;
        }

        [TestMethod]
        public void TestTvEpisodeExtrasNone()
        {
            TvEpisode tvEpisode = _config.Client.GetTvEpisode(BreakingBad, 1, 1);

            TestBreakingBadSeasonOneEpisodeOneBaseProperties(tvEpisode);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvEpisode, object> selector in _methods.Values)
            {
                Assert.IsNull(selector(tvEpisode));
            }
        }

        [TestMethod]
        public void TestTvEpisodeExtrasAll()
        {
            TvEpisodeMethods combinedEnum = _methods.Keys.Aggregate((methods, tvEpisodeMethods) => methods | tvEpisodeMethods);
            TvEpisode tvEpisode = _config.Client.GetTvEpisode(BreakingBad, 1, 1, combinedEnum);

            TestBreakingBadSeasonOneEpisodeOneBaseProperties(tvEpisode);

            Assert.IsNotNull(tvEpisode.Images);
            Assert.IsNotNull(tvEpisode.Images.Stills);
            Assert.IsTrue(tvEpisode.Images.Stills.Count > 0);

            TestMethodsHelper.TestAllNotNull(_methods, tvEpisode);
        }

        [TestMethod]
        public void TestTvEpisodeExtrasExclusive()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => _config.Client.GetTvEpisode(id, 1, 1, extras), BreakingBad);
        }

        [TestMethod]
        public void TestTvEpisodeSeparateExtrasCredits()
        {
            Credits credits = _config.Client.GetTvEpisodeCredits(BreakingBad, 1, 1);
            Assert.IsNotNull(credits);
            Assert.IsNotNull(credits.Cast);
            Assert.AreEqual("Walter White", credits.Cast[0].Character);
            Assert.AreEqual("52542282760ee313280017f9", credits.Cast[0].CreditId);
            Assert.AreEqual(17419, credits.Cast[0].Id);
            Assert.AreEqual("Bryan Cranston", credits.Cast[0].Name);
            Assert.IsNotNull(credits.Cast[0].ProfilePath);
            Assert.AreEqual(0, credits.Cast[0].Order);

            Crew crewPersonId = credits.Crew.FirstOrDefault(s => s.Id == 1280071);
            Assert.IsNotNull(crewPersonId);

            Assert.AreEqual(1280071, crewPersonId.Id);
            Assert.AreEqual("Editing", crewPersonId.Department);
            Assert.AreEqual("Lynne Willingham", crewPersonId.Name);
            Assert.AreEqual("Editor", crewPersonId.Job);
            Assert.IsNull(crewPersonId.ProfilePath);
        }

        [TestMethod]
        public void TestTvEpisodeSeparateExtrasExternalIds()
        {
            ExternalIds externalIds = _config.Client.GetTvEpisodeExternalIds(BreakingBad, 1, 1);
            Assert.IsNotNull(externalIds);
            Assert.IsTrue(string.IsNullOrEmpty(externalIds.FreebaseId));
            Assert.AreEqual(62085, externalIds.Id);
            Assert.AreEqual("/m/03mb620", externalIds.FreebaseMid);
            Assert.AreEqual("tt0959621", externalIds.ImdbId);
            Assert.AreEqual(637041, externalIds.TvrageId);
            Assert.AreEqual(349232, externalIds.TvdbId);
        }

        [TestMethod]
        public void TestTvEpisodeSeparateExtrasImages()
        {
            StillImages images = _config.Client.GetTvEpisodeImages(BreakingBad, 1, 1);
            Assert.IsNotNull(images);
            Assert.IsNotNull(images.Stills);
        }

        [TestMethod]
        public void TestTvEpisodeSeparateExtrasVideos()
        {
            ResultContainer<Video> images = _config.Client.GetTvEpisodeVideos(BreakingBad, 1, 1);
            Assert.IsNotNull(images);
            Assert.IsNotNull(images.Results);
        }

        [TestMethod]
        public void TestTvEpisodeAccountStateRatingSet()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            TvEpisodeAccountState accountState = _config.Client.GetTvEpisodeAccountState(BreakingBad, 1, 1);

            int id = accountState.Id;

            // Remove the rating
            _config.Client.TvEpisodeSetRating(BreakingBad, 1, 1, 2);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is rated correctly
            accountState = _config.Client.GetTvEpisodeAccountState(BreakingBad, 1, 1);

            Assert.AreEqual(id, accountState.Id);
            Assert.IsTrue(accountState.Rating.HasValue);
            Assert.IsTrue(Math.Abs(2 - accountState.Rating.Value) < double.Epsilon);

            // Rate the movie
            _config.Client.TvEpisodeSetRating(BreakingBad, 1, 1, 7);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is rated correctly
            accountState = _config.Client.GetTvEpisodeAccountState(BreakingBad, 1, 1);
            Assert.IsTrue(accountState.Rating.HasValue);
            Assert.IsTrue(Math.Abs(7 - accountState.Rating.Value) < double.Epsilon);
        }

        [TestMethod]
        public void TestTvEpisodeRateBad()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);

            Assert.IsFalse(_config.Client.TvEpisodeSetRating(BreakingBad, 1, 1, -1));
            Assert.IsFalse(_config.Client.TvEpisodeSetRating(BreakingBad, 1, 1, 0));
            Assert.IsFalse(_config.Client.TvEpisodeSetRating(BreakingBad, 1, 1, 10.5));
        }

        [TestMethod]
        public void TestTvEpisodeGetChanges()
        {
            ChangesContainer changes = _config.Client.GetTvEpisodeChanges(BreakingBadSeason1Episode1Id);

            Assert.IsNotNull(changes);
            Assert.IsNotNull(changes.Changes);
        }

        private void TestBreakingBadSeasonOneEpisodeOneBaseProperties(TvEpisode tvEpisode)
        {
            Assert.AreEqual(62085, tvEpisode.Id);
            Assert.AreEqual(new DateTime(2008, 1, 19), tvEpisode.AirDate.Date);
            Assert.AreEqual(1, tvEpisode.EpisodeNumber);
            Assert.AreEqual("Pilot", tvEpisode.Name);
            Assert.IsNotNull(tvEpisode.Overview);
            Assert.IsNull(tvEpisode.ProductionCode);
            Assert.AreEqual(1, tvEpisode.SeasonNumber);
            Assert.IsNotNull(tvEpisode.StillPath);

            Assert.IsNotNull(tvEpisode.Crew);
            Crew crew = tvEpisode.Crew.SingleOrDefault(s => s.CreditId == "52542275760ee313280006ce");
            Assert.IsNotNull(crew);

            Assert.AreEqual(66633, crew.Id);
            Assert.AreEqual("52542275760ee313280006ce", crew.CreditId);
            Assert.AreEqual("Vince Gilligan", crew.Name);
            Assert.AreEqual("Writing", crew.Department);
            Assert.AreEqual("Writer", crew.Job);
            Assert.AreEqual("/rLSUjr725ez1cK7SKVxC9udO03Y.jpg", crew.ProfilePath);

            Assert.IsNotNull(tvEpisode.GuestStars);
            Cast star = tvEpisode.GuestStars.SingleOrDefault(s => s.CreditId == "52542273760ee3132800068e");
            Assert.IsNotNull(star);

            Assert.AreEqual(92495, star.Id);
            Assert.AreEqual("John Koyama", star.Name);
            Assert.AreEqual("52542273760ee3132800068e", star.CreditId);
            Assert.AreEqual("Emilio Koyama", star.Character);
            Assert.AreEqual(1, star.Order);
            Assert.AreEqual("/uh4g85qbQGZZ0HH6IQI9fM9VUGS.jpg", star.ProfilePath);

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
