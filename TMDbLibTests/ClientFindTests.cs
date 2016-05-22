using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Find;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientFindTests : TestBase
    {
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public override void Initiator()
        {
            base.Initiator();

            _config = new TestConfig();
        }

        [TestMethod]
        public void TestFindImdbMovie()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbTerminatorId);
            Assert.AreEqual(1, result.Result.MovieResults.Count);
            Assert.AreEqual(IdHelper.TmdbTerminatorId, result.Result.MovieResults[0].Id);
        }

        [TestMethod]
        public void TestFindImdbPerson()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBruceWillis);
            Assert.AreEqual(1, result.Result.PersonResults.Count);
            Assert.AreEqual(IdHelper.BruceWillis, result.Result.PersonResults[0].Id);
        }

        [TestMethod]
        public void TestFindImdbTvShowEpisode()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBreakingBadSeason1Episode1Id);
            Assert.AreEqual(1, result.Result.TvEpisode.Count);
            Assert.AreEqual(IdHelper.BreakingBadSeason1Episode1Id, result.Result.TvEpisode[0].Id);
        }

        [TestMethod]
        public void TestFindImdbTvShowSeason()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.TvDb, IdHelper.TvdbBreakingBadSeason1Id);
            Assert.AreEqual(1, result.Result.TvEpisode.Count);

            Assert.AreEqual(1, result.Result.TvSeason.Count);
            Assert.AreEqual(IdHelper.BreakingBadSeason1Id, result.Result.TvSeason[0].Id);
        }

        [TestMethod]
        public void TestFindTvdbTvShow()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.TvDb, IdHelper.TvdbBreakingBadId);
            Assert.AreEqual(1, result.Result.TvResults.Count);
            Assert.AreEqual(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindImdbTvShow()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBreakingBadId);
            Assert.AreEqual(1, result.Result.TvResults.Count);
            Assert.AreEqual(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindTvRageTvShow()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.TvRage, IdHelper.TvRageBreakingBadId);
            Assert.AreEqual(1, result.Result.TvResults.Count);
            Assert.AreEqual(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindFreebaseTvShow()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.FreeBaseId, IdHelper.FreebaseBreakingBadId);
            Assert.AreEqual(1, result.Result.TvResults.Count);
            Assert.AreEqual(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindFreebaseMidTvShow()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.FreeBaseMid, IdHelper.FreebaseMidBreakingBadId);
            Assert.AreEqual(1, result.Result.TvResults.Count);
            Assert.AreEqual(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }
    }
}
