using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb genre functionality.
/// </summary>
public class ClientGenreTests : TestBase
{
    /// <summary>
    /// Tests that TV genre lists can be retrieved in different languages.
    /// </summary>
    [Fact]
    public async Task TestGenreTvListAsync()
    {
        // Default language
        List<Genre> genres = await TMDbClient.GetTvGenresAsync();

        // Another language
        List<Genre> genresDanish = await TMDbClient.GetTvGenresAsync("da");

        await Verify(new
        {
            genres,
            genresDanish
        });

        // At least one should be different
        Genre actionEn = genres.Single(s => s.Id == IdHelper.ActionAdventureTvGenre);
        Genre actionDa = genresDanish.Single(s => s.Id == IdHelper.ActionAdventureTvGenre);

        Assert.NotEqual(actionEn.Name, actionDa.Name);
    }
    /// <summary>
    /// Tests that movie genre lists can be retrieved in different languages.
    /// </summary>
    [Fact]
    public async Task TestGenreMovieListAsync()
    {
        // Default language
        List<Genre> genres = await TMDbClient.GetMovieGenresAsync();

        // Another language
        List<Genre> genresDanish = await TMDbClient.GetMovieGenresAsync("da");

        await Verify(new
        {
            genres,
            genresDanish
        });

        // At least one should be different
        Genre actionEn = genres.Single(s => s.Id == IdHelper.AdventureMovieGenre);
        Genre actionDa = genresDanish.Single(s => s.Id == IdHelper.AdventureMovieGenre);

        Assert.NotEqual(actionEn.Name, actionDa.Name);
    }
    /// <summary>
    /// Tests that movies can be retrieved by genre ID.
    /// </summary>
    [Fact]
    public async Task TestGenreMoviesAsync()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        SearchContainerWithId<SearchMovie> movies = await TMDbClient.GetGenreMoviesAsync(IdHelper.AdventureMovieGenre);

        Assert.NotEmpty(movies.Results);
        Assert.Equal(IdHelper.AdventureMovieGenre, movies.Id);
        Assert.All(movies.Results, x => Assert.Contains(IdHelper.AdventureMovieGenre, x.GenreIds));
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
