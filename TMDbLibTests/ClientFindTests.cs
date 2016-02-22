using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Find;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientFindTests
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
        public void TestFindImdbMovie()
        {
            var result = _config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.imdbTerminatorId);
            Assert.AreEqual(1, result.Result.MovieResults.Count);
            Assert.AreEqual(IdHelper.tmdbTerminatorId, result.Result.MovieResults[0].Id);
        }

        [TestMethod]
        public void TestFindTvdbTvShow()
        {
            var result = _config.Client.FindAsync(FindExternalSource.TvDb, IdHelper.TvdbBreakingBadId);
            Assert.AreEqual(1, result.Result.TvResults.Count);
            Assert.AreEqual(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindImdbTvShow()
        {
            var result = _config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBreakingBadId);
            Assert.AreEqual(1, result.Result.TvResults.Count);
            Assert.AreEqual(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindTvRageTvShow()
        {
            var result = _config.Client.FindAsync(FindExternalSource.TvRage, IdHelper.TvRageBreakingBadId);
            Assert.AreEqual(1, result.Result.TvResults.Count);
            Assert.AreEqual(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindFreebaseTvShow()
        {
            var result = _config.Client.FindAsync(FindExternalSource.FreeBaseId, IdHelper.FreebaseBreakingBadId);
            Assert.AreEqual(1, result.Result.TvResults.Count);
            Assert.AreEqual(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindFreebaseMidTvShow()
        {
            var result = _config.Client.FindAsync(FindExternalSource.FreeBaseMid, IdHelper.FreebaseMidBreakingBadId);
            Assert.AreEqual(1, result.Result.TvResults.Count);
            Assert.AreEqual(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }
    }
}
