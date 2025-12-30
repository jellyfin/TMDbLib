using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Find;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb find functionality.
/// </summary>
public class ClientFindTests : TestBase
{
    /// <summary>
    /// Tests that finding a movie by IMDb ID returns the expected movie data.
    /// </summary>
    [Fact]
    public async Task TestFindImdbMovie()
    {
        var result = await TMDbClient.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbTerminatorId);

        await Verify(result);
    }

    /// <summary>
    /// Tests that finding a person by IMDb ID returns the expected person data.
    /// </summary>
    [Fact]
    public async Task TestFindImdbPerson()
    {
        var result = await TMDbClient.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBruceWillis);

        await Verify(result);
    }

    /// <summary>
    /// Tests that finding a TV episode by IMDb ID returns the expected episode data.
    /// </summary>
    [Fact]
    public async Task TestFindImdbTvShowEpisode()
    {
        var result = await TMDbClient.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBreakingBadSeason1Episode1Id);

        await Verify(result);
    }

    /// <summary>
    /// Tests that finding a TV season by TVDb ID returns the expected season data.
    /// </summary>
    [Fact]
    public async Task TestFindImdbTvShowSeasonAsync()
    {
        var result = await TMDbClient.FindAsync(FindExternalSource.TvDb, IdHelper.TvdbBreakingBadSeason1Id);

        await Verify(result);
    }

    /// <summary>
    /// Tests that finding a TV show by TVDb ID returns the expected show data.
    /// </summary>
    [Fact]
    public async Task TestFindTvdbTvShowAsync()
    {
        var result = await TMDbClient.FindAsync(FindExternalSource.TvDb, IdHelper.TvdbBreakingBadId);

        await Verify(result);
    }

    /// <summary>
    /// Tests that finding a TV show by IMDb ID returns the expected show data.
    /// </summary>
    [Fact]
    public async Task TestFindImdbTvShowAsync()
    {
        var result = await TMDbClient.FindAsync(FindExternalSource.Imdb, IdHelper.ImdbBreakingBadId);

        await Verify(result);
    }
}
