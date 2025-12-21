using System;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb search functionality.
/// </summary>
public class ClientSearchTests : TestBase
{
    /// <summary>
    /// Tests that movies can be searched by title with optional year filtering.
    /// </summary>
    [Fact]
    public async Task TestSearchMovieAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchMovieAsync("007", i));

        // Search pr. Year
        // 1962: First James Bond movie, "Dr. No"
        var result = await TMDbClient.SearchMovieAsync("007", year: 1962);
        var item = result.Results.Single(s => s.Id == 646);

        await Verify(item);

        TestImagesHelpers.TestImagePaths([item.BackdropPath, item.PosterPath]);
    }

    /// <summary>
    /// Tests that movie collections can be searched by name.
    /// </summary>
    [Fact]
    public async Task TestSearchCollectionAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchCollectionAsync("007", i));

        var result = await TMDbClient.SearchCollectionAsync("James Bond");
        var item = result.Results.Single(s => s.Id == 645);

        await Verify(item);

        TestImagesHelpers.TestImagePaths([item.BackdropPath, item.PosterPath]);
    }

    /// <summary>
    /// Tests that people can be searched by name.
    /// </summary>
    [Fact]
    public async Task TestSearchPersonAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchPersonAsync("Willis", i));

        var result = await TMDbClient.SearchPersonAsync("Willis");
        var item = result.Results.Single(s => s.Id == 62);

        await Verify(item);

        TestImagesHelpers.TestImagePaths([item.ProfilePath]);
    }

    /// <summary>
    /// Tests that production companies can be searched by name.
    /// </summary>
    [Fact]
    public async Task TestSearchCompanyAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchCompanyAsync("Disney", i));

        var result = await TMDbClient.SearchCompanyAsync("Disney");
        var item = result.Results.FirstOrDefault(s => s.Name.Contains("Disney", StringComparison.OrdinalIgnoreCase));

        Assert.NotNull(item);
        await Verify(item);

        if (item.LogoPath != null)
        {
            TestImagesHelpers.TestImagePaths([item.LogoPath]);
        }
    }

    /// <summary>
    /// Tests that keywords can be searched by name.
    /// </summary>
    [Fact]
    public async Task TestSearchKeywordAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchKeywordAsync("plot", i));

        var result = await TMDbClient.SearchKeywordAsync("plot");
        var item = result.Results.FirstOrDefault(s => s.Name.Contains("plot", StringComparison.OrdinalIgnoreCase));

        Assert.NotNull(item);
        await Verify(item);
    }

    /// <summary>
    /// Tests that TV shows can be searched by name.
    /// </summary>
    [Fact]
    public async Task TestSearchTvShowAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchTvShowAsync("Breaking Bad", i));

        var result = await TMDbClient.SearchTvShowAsync("Breaking Bad");
        var item = result.Results.Single(s => s.Id == 1396);

        await Verify(item);

        TestImagesHelpers.TestImagePaths([item.BackdropPath, item.PosterPath]);
    }

    /// <summary>
    /// Tests that multi-search can query movies, TV shows, and people simultaneously.
    /// </summary>
    [Fact]
    public async Task TestSearchMultiAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchMultiAsync("Arrow", i));

        var result = await TMDbClient.SearchMultiAsync("Arrow");
        var item = result.Results.OfType<SearchTv>().Single(s => s.Id == 1412);

        await Verify(item);

        TestImagesHelpers.TestImagePaths([item.BackdropPath, item.PosterPath]);
    }
}
