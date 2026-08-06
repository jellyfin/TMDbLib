using System.Threading.Tasks;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb network functionality.
/// </summary>
public class ClientNetworkTests : TestBase
{
    /// <summary>
    /// Tests that a network can be retrieved by ID.
    /// </summary>
    [Fact]
    public async Task TestNetworkGetByIdAsync()
    {
        var network = await TMDbClient.GetNetworkAsync(IdHelper.Netflix, cancellationToken: TestContext.Current.CancellationToken);

        await Verify(network);
    }

    /// <summary>
    /// Tests that logos for a network can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestNetworkImagesAsync()
    {
        var logos = await TMDbClient.GetNetworkImagesAsync(IdHelper.Netflix, cancellationToken: TestContext.Current.CancellationToken);

        await Verify(logos);
    }

    /// <summary>
    /// Tests that alternative names for a network can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestNetworkAlternativeNamesAsync()
    {
        var names = await TMDbClient.GetNetworkAlternativeNamesAsync(IdHelper.AMC, cancellationToken: TestContext.Current.CancellationToken);

        await Verify(names);
    }

    /// <summary>
    /// Verifies that retrieving a non-existent network returns null.
    /// </summary>
    [Fact]
    public async Task TestNetworkMissingAsync()
    {
        var network = await TMDbClient.GetNetworkAsync(IdHelper.MissingID, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Null(network);
    }
}
