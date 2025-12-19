using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb trending functionality.
/// </summary>
public class ClientTrendingTests : TestBase
{
    /// <summary>
    /// Tests that trending movies can be retrieved for a weekly time window.
    /// </summary>
    [Fact]
    public async Task TestTrendingMoviesAsync()
    {
        SearchContainer<SearchMovie> movies = await TMDbClient.GetTrendingMoviesAsync(TimeWindow.Week);
        Assert.NotEmpty(movies.Results);
    }

    /// <summary>
    /// Tests that trending TV shows can be retrieved for a weekly time window.
    /// </summary>
    [Fact]
    public async Task TestTrendingTvAsync()
    {
        SearchContainer<SearchTv> tv = await TMDbClient.GetTrendingTvAsync(TimeWindow.Week);
        Assert.NotEmpty(tv.Results);
    }

    /// <summary>
    /// Tests that trending people can be retrieved for a weekly time window.
    /// </summary>
    [Fact]
    public async Task TestTrendingPeopleAsync()
    {
        SearchContainer<SearchPerson> people = await TMDbClient.GetTrendingPeopleAsync(TimeWindow.Week);
        Assert.NotEmpty(people.Results);
    }

    /// <summary>
    /// Tests that trending items of all media types can be retrieved for a weekly time window.
    /// </summary>
    [Fact]
    public async Task TestTrendingAllAsync()
    {
        SearchContainer<SearchBase> all = await TMDbClient.GetTrendingAllAsync(TimeWindow.Week);
        Assert.NotEmpty(all.Results);
    }
}
