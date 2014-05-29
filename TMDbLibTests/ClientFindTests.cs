using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Find;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientFindTests
    {
        private TestConfig _config;
        private const int tmdbTerminatorId = 218;
        private const string imdbTerminatorId = "tt0088247";
        private const int tmdbBreakingBadId = 1396;
        private const string tvdbBreakingBadId = "81189";
        private const string imdbBreakingBadId = "tt0903747";
        private const string tvRageBreakingBadId = "18164";
        private const string freebaseBreakingBadId = "en/breaking_bad";
        private const string freebaseMidBreakingBadId = "m/03d34x8";

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
            FindContainer result = _config.Client.Find(FindExternalSource.Imdb, imdbTerminatorId);
            Assert.AreEqual(1, result.MovieResults.Count);
            Assert.AreEqual(tmdbTerminatorId, result.MovieResults[0].Id);
        }

        [TestMethod]
        public void TestFindTvdbTvShow()
        {
            FindContainer result = _config.Client.Find(FindExternalSource.TvDb, tvdbBreakingBadId);
            Assert.AreEqual(1, result.TvResults.Count);
            Assert.AreEqual(tmdbBreakingBadId, result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindImdbTvShow()
        {
            FindContainer result = _config.Client.Find(FindExternalSource.Imdb, imdbBreakingBadId);
            Assert.AreEqual(1, result.TvResults.Count);
            Assert.AreEqual(tmdbBreakingBadId, result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindTvRageTvShow()
        {
            FindContainer result = _config.Client.Find(FindExternalSource.TvRage, tvRageBreakingBadId);
            Assert.AreEqual(1, result.TvResults.Count);
            Assert.AreEqual(tmdbBreakingBadId, result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindFreebaseTvShow()
        {
            FindContainer result = _config.Client.Find(FindExternalSource.FreeBaseId, freebaseBreakingBadId);
            Assert.AreEqual(1, result.TvResults.Count);
            Assert.AreEqual(tmdbBreakingBadId, result.TvResults[0].Id);
        }

        [TestMethod]
        public void TestFindFreebaseMidTvShow()
        {
            FindContainer result = _config.Client.Find(FindExternalSource.FreeBaseMid, freebaseMidBreakingBadId);
            Assert.AreEqual(1, result.TvResults.Count);
            Assert.AreEqual(tmdbBreakingBadId, result.TvResults[0].Id);
        }
    }
}
