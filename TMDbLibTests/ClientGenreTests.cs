using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.General;
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
}
