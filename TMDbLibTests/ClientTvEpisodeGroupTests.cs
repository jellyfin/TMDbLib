using System.Threading.Tasks;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb TV episode group functionality.
/// </summary>
public class ClientTvEpisodeGroupTests : TestBase
{
    /// <summary>
    /// Tests that TV episode group information can be retrieved by group ID.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeGroups()
    {
        TvGroupCollection group = await TMDbClient.GetTvEpisodeGroupsAsync("5acf93e60e0a26346d0000ce");

        await Verify(group);
    }
}
