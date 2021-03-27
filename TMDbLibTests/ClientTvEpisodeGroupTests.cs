using System.Threading.Tasks;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests
{
    public class ClientTvEpisodeGroupTests : TestBase
    {
        [Fact]
        public async Task TestTvEpisodeGroups()
        {
            TvGroupCollection group = await TMDbClient.GetTvEpisodeGroupsAsync("5acf93e60e0a26346d0000ce");

            await Verify(group);
        }
    }
}