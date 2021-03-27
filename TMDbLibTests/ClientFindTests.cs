using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Find;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientFindTests : TestBase
    {
        [Fact]
        public void TestFindImdbMovie()
        {
            Task<FindContainer> result = Config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbTerminatorId);
            Assert.Single(result.Result.MovieResults);
            Assert.Equal(IdHelper.TmdbTerminatorId, result.Result.MovieResults[0].Id);
        }

        [Fact]
        public void TestFindImdbPerson()
        {
            Task<FindContainer> result = Config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBruceWillis);
            Assert.Single(result.Result.PersonResults);
            Assert.Equal(IdHelper.BruceWillis, result.Result.PersonResults[0].Id);
        }

        [Fact]
        public void TestFindImdbTvShowEpisode()
        {
            Task<FindContainer> result = Config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBreakingBadSeason1Episode1Id);
            Assert.Single(result.Result.TvEpisode);
            Assert.Equal(IdHelper.BreakingBadSeason1Episode1Id, result.Result.TvEpisode[0].Id);
        }

        [Fact]
        public void TestFindImdbTvShowSeason()
        {
            Task<FindContainer> result = Config.Client.FindAsync(FindExternalSource.TvDb, IdHelper.TvdbBreakingBadSeason1Id);
            Assert.Single(result.Result.TvEpisode);

            Assert.Single(result.Result.TvSeason);
            Assert.Equal(IdHelper.BreakingBadSeason1Id, result.Result.TvSeason[0].Id);
        }

        [Fact]
        public void TestFindTvdbTvShow()
        {
            Task<FindContainer> result = Config.Client.FindAsync(FindExternalSource.TvDb, IdHelper.TvdbBreakingBadId);
            Assert.Single(result.Result.TvResults);
            Assert.Equal(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }

        [Fact]
        public void TestFindImdbTvShow()
        {
            Task<FindContainer> result = Config.Client.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBreakingBadId);
            Assert.Single(result.Result.TvResults);
            Assert.Equal(IdHelper.TmdbBreakingBadId, result.Result.TvResults[0].Id);
        }
    }
}