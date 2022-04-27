using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests
{
    public class ClientWatchProvidersTests : TestBase
    {
        [Fact]
        public async Task TestGetRegions()
        {
            ResultContainer<WatchProviderRegion> watchProviderRegions = await TMDbClient.GetWatchProviderRegionsAsync();

            await Verify(watchProviderRegions);
        }

        [Fact]
        public async Task TestGetMovieWatchProviders()
        {
            ResultContainer<WatchProviderItem> watchProviders = await TMDbClient.GetMovieWatchProvidersAsync();

            await Verify(watchProviders);
        }

        [Fact]
        public async Task TestGetTvWatchProviders()
        {
            ResultContainer<WatchProviderItem> watchProviders = await TMDbClient.GetTvWatchProvidersAsync();

            await Verify(watchProviders);
        }
    }
}