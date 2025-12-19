using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb keyword functionality.
/// </summary>
public class ClientKeywordTests : TestBase
{
    /// <summary>
    /// Tests that keywords for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetMovieKeywordsAsync()
    {
        KeywordsContainer keywords = await TMDbClient.GetMovieKeywordsAsync(IdHelper.AGoodDayToDieHard);

        await Verify(keywords);
    }

    /// <summary>
    /// Tests that keywords for a TV show can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetTvShowKeywordsAsync()
    {
        ResultContainer<Keyword> keywords = await TMDbClient.GetTvShowKeywordsAsync(IdHelper.BigBangTheory);

        await Verify(keywords);
    }

    /// <summary>
    /// Tests that a single keyword can be retrieved by ID.
    /// </summary>
    [Fact]
    public async Task TestKeywordGetSingle()
    {
        Keyword keyword = await TMDbClient.GetKeywordAsync(IdHelper.AgentKeyword);

        await Verify(keyword);
    }

    /// <summary>
    /// Verifies that retrieving keywords for a non-existent movie returns null.
    /// </summary>
    [Fact]
    public async Task TestKeywordsMissing()
    {
        KeywordsContainer keywords = await TMDbClient.GetMovieKeywordsAsync(IdHelper.MissingID);

        Assert.Null(keywords);
    }

    /// <summary>
    /// Tests that movies associated with a keyword can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestKeywordMovies()
    {
        SearchContainerWithId<SearchMovie> movies = await TMDbClient.GetKeywordMoviesAsync(IdHelper.AgentKeyword);

        Assert.Equal(IdHelper.AgentKeyword, movies.Id);
        Assert.NotEmpty(movies.Results);

        KeywordsContainer movie = await TMDbClient.GetMovieKeywordsAsync(movies.Results.First().Id);

        Assert.Contains(movie.Keywords, x => IdHelper.AgentKeyword == x.Id);
    }
}
