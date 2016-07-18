using Xunit;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientNetworkTests : TestBase
    {
        [Fact]
        public void TestNetworkGetById()
        {
            Network network = Config.Client.GetNetworkAsync(IdHelper.Hbo).Result;

            Assert.NotNull(network);
            Assert.Equal("HBO", network.Name);
            Assert.Equal(IdHelper.Hbo, network.Id);
        }
    }
}
