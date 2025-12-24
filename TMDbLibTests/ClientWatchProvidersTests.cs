using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb watch providers functionality.
/// </summary>
public class ClientWatchProvidersTests : TestBase
{
    /// <summary>
    /// Tests that all available watch provider regions can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetRegions()
    {
        var watchProviderRegions = await TMDbClient.GetWatchProviderRegionsAsync();

        await Verify(watchProviderRegions);
    }

    /// <summary>
    /// Tests that all movie watch providers can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetMovieWatchProviders()
    {
        var watchProviders = await TMDbClient.GetMovieWatchProvidersAsync();

        await Verify(watchProviders);
    }

    /// <summary>
    /// Tests that all TV watch providers can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetTvWatchProviders()
    {
        var watchProviders = await TMDbClient.GetTvWatchProvidersAsync();

        await Verify(watchProviders);
    }
}
