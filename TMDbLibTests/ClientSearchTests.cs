using Xunit;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using System.Linq;
using System.Threading.Tasks;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

public class ClientSearchTests : TestBase
{
    [Fact]
    public async Task TestSearchMovieAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchMovieAsync("007", i));

        // Search pr. Year
        // 1962: First James Bond movie, "Dr. No"
        SearchContainer<SearchMovie> result = await TMDbClient.SearchMovieAsync("007", year: 1962);
        SearchMovie item = result.Results.Single(s => s.Id == 646);

        await Verify(item);

        TestImagesHelpers.TestImagePaths([item.BackdropPath, item.PosterPath]);
    }
    [Fact]
    public async Task TestSearchCollectionAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchCollectionAsync("007", i));

        SearchContainer<SearchCollection> result = await TMDbClient.SearchCollectionAsync("James Bond");
        SearchCollection item = result.Results.Single(s => s.Id == 645);

        await Verify(item);

        TestImagesHelpers.TestImagePaths([item.BackdropPath, item.PosterPath]);
    }
    [Fact]
    public async Task TestSearchPersonAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchPersonAsync("Willis", i));

        SearchContainer<SearchPerson> result = await TMDbClient.SearchPersonAsync("Willis");
        SearchPerson item = result.Results.Single(s => s.Id == 62);

        await Verify(item);

        TestImagesHelpers.TestImagePaths([item.ProfilePath]);
    }
    [Fact]
    public async Task TestSearchCompanyAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchCompanyAsync("20th", i));

        SearchContainer<SearchCompany> result = await TMDbClient.SearchCompanyAsync("20th");
        SearchCompany item = result.Results.Single(s => s.Id == 25);

        await Verify(item);

        TestImagesHelpers.TestImagePaths([item.LogoPath]);
    }
    [Fact]
    public async Task TestSearchKeywordAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchKeywordAsync("plot", i));

        SearchContainer<SearchKeyword> result = await TMDbClient.SearchKeywordAsync("plot");
        SearchKeyword item = result.Results.Single(s => s.Id == 11121);

        await Verify(item);
    }
    [Fact]
    public async Task TestSearchTvShowAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchTvShowAsync("Breaking Bad", i));

        SearchContainer<SearchTv> result = await TMDbClient.SearchTvShowAsync("Breaking Bad");
        SearchTv item = result.Results.Single(s => s.Id == 1396);

        await Verify(item);

        TestImagesHelpers.TestImagePaths([item.BackdropPath, item.PosterPath]);
    }
    [Fact]
    public async Task TestSearchMultiAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchMultiAsync("Arrow", i));

        SearchContainer<SearchBase> result = await TMDbClient.SearchMultiAsync("Arrow");
        SearchTv item = result.Results.OfType<SearchTv>().Single(s => s.Id == 1412);

        await Verify(item);

        TestImagesHelpers.TestImagePaths([item.BackdropPath, item.PosterPath]);
    }
}
