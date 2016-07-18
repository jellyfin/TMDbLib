using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Find;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientFindTests : TestBase
    {
        private readonly TestConfig _config;

        public ClientFindTests()
        {
            _config = new TestConfig();
        }

        [Fact]
        public void TestFindImdbMovie()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbTerminatorId);
            Assert.Equal(1, result.Result.MovieResults.Count);
            Assert.Equal(IdHelper.TmdbTerminatorId, result.Result.MovieResults[0].Id);
        }

        [Fact]
        public void TestFindImdbPerson()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBruceWillis);
            Assert.Equal(1, result.Result.PersonResults.Count);
            Assert.Equal(IdHelper.BruceWillis, result.Result.PersonResults[0].Id);
        }

        [Fact]
        public void TestFindImdbTvShowEpisode()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBreakingBadSeason1Episode1Id);
            Assert.Equal(1, result.Result.TvEpisode.Count);
            Assert.Equal(IdHelper.BreakingBadSeason1Episode1Id, result.Result.TvEpisode[0].Id);
        }

        [Fact]
        public void TestFindImdbTvShowSeason()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.TvDb, IdHelper.TvdbBreakingBadSeason1Id);
            Assert.Equal(1, result.Result.TvEpisode.Count);

            Assert.Equal(1, result.Result.TvSeason.Count);
            Assert.Equal(IdHelper.BreakingBadSeason1Id, result.Result.TvSeason[0].Id);
        }

        [Fact]
        public void TestFindTvdbTvShow()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.TvDb, IdHelper.TvdbBreakingBadId);
            Assert.Equal(1, result.Result.TvResults.Count);
            Assert.Equal(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [Fact]
        public void TestFindImdbTvShow()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBreakingBadId);
            Assert.Equal(1, result.Result.TvResults.Count);
            Assert.Equal(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [Fact]
        public void TestFindTvRageTvShow()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.TvRage, IdHelper.TvRageBreakingBadId);
            Assert.Equal(1, result.Result.TvResults.Count);
            Assert.Equal(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [Fact]
        public void TestFindFreebaseTvShow()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.FreeBaseId, IdHelper.FreebaseBreakingBadId);
            Assert.Equal(1, result.Result.TvResults.Count);
            Assert.Equal(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [Fact]
        public void TestFindFreebaseMidTvShow()
        {
            Task<FindContainer> result = _config.Client.FindAsync(FindExternalSource.FreeBaseMid, IdHelper.FreebaseMidBreakingBadId);
            Assert.Equal(1, result.Result.TvResults.Count);
            Assert.Equal(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }
    }
}
