using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Find;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

public class ClientFindTests : TestBase
{
    [Fact]
    public async Task TestFindImdbMovie()
    {
        FindContainer result = await TMDbClient.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbTerminatorId);

        await Verify(result);
    }
    [Fact]
    public async Task TestFindImdbPerson()
    {
        FindContainer result = await TMDbClient.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBruceWillis);

        await Verify(result);
    }
    [Fact]
    public async Task TestFindImdbTvShowEpisode()
    {
        FindContainer result = await TMDbClient.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBreakingBadSeason1Episode1Id);

        await Verify(result);
    }
    [Fact]
    public async Task TestFindImdbTvShowSeasonAsync()
    {
        FindContainer result = await TMDbClient.FindAsync(FindExternalSource.TvDb, IdHelper.TvdbBreakingBadSeason1Id);

        await Verify(result);
    }
    [Fact]
    public async Task TestFindTvdbTvShowAsync()
    {
        FindContainer result = await TMDbClient.FindAsync(FindExternalSource.TvDb, IdHelper.TvdbBreakingBadId);

        await Verify(result);
    }
    [Fact]
    public async Task TestFindImdbTvShowAsync()
    {
        FindContainer result = await TMDbClient.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBreakingBadId);

        await Verify(result);
    }
}
