using TMDbLib.Objects.General;
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

        [Fact]
        public void TestNetworkImages()
        {
            NetworkLogos logos = Config.Client.GetNetworkImagesAsync(IdHelper.Netflix).Result;

            Assert.NotNull(logos);
            Assert.Equal(IdHelper.Netflix, logos.Id);
            Assert.Equal("/wwemzKWzjKYJFfCeiB57q3r4Bcm.png", logos.Logos[0].FilePath);
        }

        [Fact]
        public void TestNetworkAlternativeNames()
        {
            AlternativeNames names = Config.Client.GetNetworkAlternativeNamesAsync(IdHelper.AMC).Result;

            Assert.NotNull(names);
            Assert.Equal(IdHelper.AMC, names.Id);
            Assert.Equal("American Movie Classics", names.Results[0].Name);
            Assert.Equal("1984–2002", names.Results[0].Type);
        }

        [Fact]
        public void TestNetworkMissing()
        {
            Network network = Config.Client.GetNetworkAsync(IdHelper.MissingID).Result;

            Assert.Null(network);
        }
    }
}