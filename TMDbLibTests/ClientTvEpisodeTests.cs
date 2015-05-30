using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientTvEpisodeTests
    {
        private const int BreakingBad = 1396;

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
        public void TestTvSeasonExtrasAll()
        {
            TvEpisodeMethods combinedEnum = _methods.Keys.Aggregate((methods, tvEpisodeMethods) => methods | tvEpisodeMethods);
            TvEpisode tvEpisode = _config.Client.GetTvEpisode(BreakingBad, 1, 1, combinedEnum);

            TestBreakingBadSeasonOneEpisodeOneBaseProperties(tvEpisode);

            Assert.IsNotNull(tvEpisode.Images);
            Assert.IsNotNull(tvEpisode.Images.Stills);
            Assert.IsTrue(tvEpisode.Images.Stills.Count > 0);

            TestMethodsHelper.TestAllNotNull(_methods, tvEpisode);
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
