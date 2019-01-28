using System.Linq;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests
{
    public class ClientTvEpisodeGroupTests : TestBase
    {
        [Fact]
        public void TestTvEpisodeGroups()
        {
            var group = Config.Client.GetTvEpisodeGroupsAsync("5acf93e60e0a26346d0000ce");

            Assert.Equal("5acf93e60e0a26346d0000ce", group.Result.Id);
            Assert.Equal("Netflix Collections", group.Result.Name);
            Assert.Equal("Netflix", group.Result.Network.Name);
            Assert.Equal("Comedians in Cars organized in Netflix's collections.", group.Result.Description);
            Assert.Equal(71, group.Result.EpisodeCount);
            Assert.Equal(5, group.Result.GroupCount);
            Assert.Equal(TvGroupType.Digital, group.Result.Type);

            Assert.Equal("5acf93efc3a368739a0000a9", group.Result.Groups.First().Id);
            Assert.Equal(1078262, group.Result.Groups.First().Episodes.First().Id);
        }
    }
}