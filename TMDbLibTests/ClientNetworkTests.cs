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
            Network network = Config.Client.GetNetworkAsync(IdHelper.Netflix).Result;

            Assert.NotNull(network);
            Assert.Equal("Netflix", network.Name);
            Assert.Equal(IdHelper.Netflix, network.Id);
            Assert.Equal("http://www.netflix.com", network.Homepage);
            Assert.Equal("Los Gatos, California, United States", network.Headquarters);
        }
    }
}