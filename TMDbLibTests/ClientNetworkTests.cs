using System.Threading.Tasks;
using TMDbLib.Objects.General;
using Xunit;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

public class ClientNetworkTests : TestBase
{
    [Fact]
    public async Task TestNetworkGetByIdAsync()
    {
        Network network = await TMDbClient.GetNetworkAsync(IdHelper.Netflix);

        await Verify(network);
    }
    [Fact]
    public async Task TestNetworkImagesAsync()
    {
        NetworkLogos logos = await TMDbClient.GetNetworkImagesAsync(IdHelper.Netflix);

        await Verify(logos);
    }
    [Fact]
    public async Task TestNetworkAlternativeNamesAsync()
    {
        AlternativeNames names = await TMDbClient.GetNetworkAlternativeNamesAsync(IdHelper.AMC);

        await Verify(names);
    }
    [Fact]
    public async Task TestNetworkMissingAsync()
    {
        Network network = await TMDbClient.GetNetworkAsync(IdHelper.MissingID);

        Assert.Null(network);
    }
}
