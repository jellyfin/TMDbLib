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
    public class ClientTvSeasonTests
    {
        private const int Dexter = 1405;
        private const int BreakingBad = 1396;

        private Dictionary<TvSeasonMethods, Func<TvSeason, object>> _methods;
        private TestConfig _config;

        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();

            _methods = new Dictionary<TvSeasonMethods, Func<TvSeason, object>>();
            _methods[TvSeasonMethods.Credits] = tvSeason => tvSeason.Credits;
            _methods[TvSeasonMethods.Images] = tvSeason => tvSeason.Images;
            _methods[TvSeasonMethods.ExternalIds] = tvSeason => tvSeason.ExternalIds;
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

            Assert.IsNotNull(credits.Crew);
            Assert.AreEqual("Production", credits.Crew[0].Department);
            Assert.AreEqual(1223202, credits.Crew[0].Id);
            Assert.AreEqual("Diane Mercer", credits.Crew[0].Name);
            Assert.AreEqual("Producer", credits.Crew[0].Job);
            Assert.IsNull(credits.Crew[0].ProfilePath);
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
        public void TestTvSeasonExtrasAll()
        {
            TvSeasonMethods combinedEnum = _methods.Keys.Aggregate((methods, tvSeasonMethods) => methods | tvSeasonMethods);
            TvSeason tvSeason = _config.Client.GetTvSeason(BreakingBad, 1, combinedEnum);

            TestBreakingBadBaseProperties(tvSeason);

            TestMethodsHelper.TestAllNotNull(_methods, tvSeason);
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
            Assert.IsNull(tvSeason.Episodes[0].Id);
            Assert.AreEqual(1, tvSeason.Episodes[0].EpisodeNumber);
            Assert.AreEqual("Pilot", tvSeason.Episodes[0].Name);
            Assert.IsNotNull(tvSeason.Episodes[0].Overview);
            Assert.IsNull(tvSeason.Episodes[0].ProductionCode);
            Assert.IsNull(tvSeason.Episodes[0].SeasonNumber);
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
