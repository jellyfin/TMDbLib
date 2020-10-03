using System.Threading.Tasks;
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
        public async Task TestNetworkGetByIdAsync()
        {
            Network network =  await TMDbClient.GetNetworkAsync(IdHelper.Netflix);

            Assert.NotNull(network);
            Assert.Equal("Netflix", network.Name);
            Assert.Equal(IdHelper.Netflix, network.Id);
            Assert.Equal("http://www.netflix.com", network.Homepage);
            Assert.Equal("Los Gatos, California, United States", network.Headquarters);
        }

        [Fact]
        public async Task TestNetworkImagesAsync()
        {
            NetworkLogos logos = await  TMDbClient.GetNetworkImagesAsync(IdHelper.Netflix);

            Assert.NotNull(logos);
            Assert.Equal(IdHelper.Netflix, logos.Id);
            Assert.Equal("/wwemzKWzjKYJFfCeiB57q3r4Bcm.png", logos.Logos[0].FilePath);
        }

        [Fact]
        public async Task TestNetworkAlternativeNamesAsync()
        {
            AlternativeNames names = await  TMDbClient.GetNetworkAlternativeNamesAsync(IdHelper.AMC);

            Assert.NotNull(names);
            Assert.Equal(IdHelper.AMC, names.Id);
            Assert.Equal("American Movie Classics", names.Results[0].Name);
            Assert.Equal("1984–2002", names.Results[0].Type);
        }

        [Fact]
        public async Task TestNetworkMissingAsync()
        {
            Network network =  await TMDbClient.GetNetworkAsync(IdHelper.MissingID);

            Assert.Null(network);
        }
    }
}