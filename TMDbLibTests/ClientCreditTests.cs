using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Credit;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientCreditTests
    {
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        [TestMethod]
        public void TestGetCreditBase()
        {
            Credit result = _config.Client.GetCredits(IdHelper.BruceWillisMiamiVice).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual("cast", result.CreditType);
            Assert.AreEqual("Actors", result.Department);
            Assert.AreEqual("Actor", result.Job);
            Assert.AreEqual("tv", result.MediaType);
            Assert.AreEqual(IdHelper.BruceWillisMiamiVice, result.Id);

            Assert.IsNotNull(result.Person);
            Assert.AreEqual("Bruce Willis", result.Person.Name);
            Assert.AreEqual(62, result.Person.Id);

            Assert.IsNotNull(result.Media);
            Assert.AreEqual(1908, result.Media.Id);
            Assert.AreEqual("Miami Vice", result.Media.Name);
            Assert.AreEqual("Miami Vice", result.Media.OriginalName);
            Assert.AreEqual("", result.Media.Character);
        }

        [TestMethod]
        public void TestGetCreditEpisode()
        {
            Credit result = _config.Client.GetCredits(IdHelper.BruceWillisMiamiVice).Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Media);
            Assert.IsNotNull(result.Media.Episodes);

            CreditEpisode item = result.Media.Episodes.SingleOrDefault(s => s.Name == "No Exit");
            Assert.IsNotNull(item);

            Assert.AreEqual(new DateTime(1984, 11, 9), item.AirDate);
            Assert.AreEqual(8, item.EpisodeNumber);
            Assert.AreEqual("No Exit", item.Name);
            Assert.AreEqual("Crockett attempts to help an old flame free herself from a racketeer, then is framed for taking bribes. Martin Castillo becomes the squad's new Lieutenant.", item.Overview);
            Assert.AreEqual(1, item.SeasonNumber);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.StillPath), "item.StillPath was not a valid image path, was: " + item.StillPath);
        }

        [TestMethod]
        public void TestGetCreditSeasons()
        {
            Credit result = _config.Client.GetCredits(IdHelper.HughLaurieHouse).Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Media);
            Assert.IsNotNull(result.Media.Seasons);

            CreditSeason item = result.Media.Seasons.SingleOrDefault(s => s.SeasonNumber == 1);
            Assert.IsNotNull(item);

            Assert.AreEqual(new DateTime(2004, 11, 16), item.AirDate);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.AreEqual(1, item.SeasonNumber);
        }
    }
}