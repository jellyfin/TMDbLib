#pragma warning disable CA2201 // Do not raise reserved exception types

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.Exceptions;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb account functionality.
/// </summary>
public class ClientAccountTests : TestBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientAccountTests"/> class.
    /// </summary>
    public ClientAccountTests()
    {
        if (string.IsNullOrWhiteSpace(TestConfig.UserSessionId))
        {
            throw new ConfigurationErrorsException("To successfully complete the ClientAccountTests you will need to specify a valid 'UserSessionId' in the test config file");
        }
    }

    /// <summary>
    /// Tests that getting account details with a guest session throws UserSessionRequiredException.
    /// </summary>
    [Fact]
    public async Task TestAccountGetDetailsGuestAccount()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

        await Assert.ThrowsAsync<UserSessionRequiredException>(() => TMDbClient.AccountGetDetailsAsync());
    }

    /// <summary>
    /// Tests that getting account details with a user session returns valid account information.
    /// </summary>
    [Fact]
    public async Task TestAccountGetDetailsUserAccount()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await Verify(TMDbClient.ActiveAccount);
    }

    /// <summary>
    /// Tests that retrieving account lists returns expected lists and can find a specific list by ID.
    /// </summary>
    [Fact]
    public async Task TestAccountAccountGetLists()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        var result = await TMDbClient.AccountGetListsAsync();

        Assert.NotEmpty(result.Results);

        var single = result.Results.Single(s => s.Id == 1724);

        await Verify(single);
    }

    /// <summary>
    /// Tests that pagination works correctly for account lists.
    /// </summary>
    [Fact]
    public async Task TestAccountAccountGetListsPaged()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestHelpers.SearchPagesAsync(i => TMDbClient.AccountGetListsAsync(i));
    }

    /// <summary>
    /// Tests that retrieving favorite movies returns expected results and supports pagination.
    /// </summary>
    [Fact]
    public async Task TestAccountGetFavoriteMovies()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestHelpers.SearchPagesAsync(i => TMDbClient.AccountGetFavoriteMoviesAsync(i));

        // Requires that you have marked at least one movie as favorite else this test will fail
        var movies = await TMDbClient.AccountGetFavoriteMoviesAsync();
        var movie = movies.Results.Single(s => s.Id == IdHelper.Avatar);

        await Verify(movie, x => x.IgnoreProperty<SearchMovie>(n => n.VoteCount, n => n.Popularity));
    }

    /// <summary>
    /// Tests that retrieving favorite TV shows returns expected results.
    /// </summary>
    [Fact]
    public async Task TestAccountGetFavoriteTv()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        var tvShows = await TMDbClient.AccountGetFavoriteTvAsync();

        // Account may or may not have favorites
        Assert.NotNull(tvShows);
        if (tvShows.Results.Count > 0)
        {
            var tvShow = tvShows.Results.First();
            Assert.True(tvShow.Id > 0);
        }
    }

    /// <summary>
    /// Tests that retrieving movie watchlist returns expected results and supports pagination.
    /// </summary>
    [Fact]
    public async Task TestAccountGetMovieWatchlist()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestHelpers.SearchPagesAsync(i => TMDbClient.AccountGetMovieWatchlistAsync(i));

        var watchlist = await TMDbClient.AccountGetMovieWatchlistAsync();
        var movie = watchlist.Results.Single(s => s.Id == 100042);

        await Verify(movie);
    }

    /// <summary>
    /// Tests that retrieving TV show watchlist returns expected results and supports pagination.
    /// </summary>
    [Fact]
    public async Task TestAccountGetTvWatchlist()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestHelpers.SearchPagesAsync(i => TMDbClient.AccountGetTvWatchlistAsync(i));

        var tvShows = await TMDbClient.AccountGetTvWatchlistAsync();
        var tvShow = tvShows.Results.Single(s => s.Id == 2691);

        await Verify(tvShow);
    }

    /// <summary>
    /// Tests that retrieving rated movies returns expected results and supports pagination.
    /// </summary>
    [Fact]
    public async Task TestAccountGetRatedMovies()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestHelpers.SearchPagesAsync(i => TMDbClient.AccountGetFavoriteMoviesAsync(i));

        var movies = await TMDbClient.AccountGetFavoriteMoviesAsync();
        var movie = movies.Results.Single(s => s.Id == IdHelper.Avatar);

        await Verify(movie);
    }

    /// <summary>
    /// Tests that retrieving rated TV shows returns expected results and supports pagination.
    /// </summary>
    [Fact]
    public async Task TestAccountGetRatedTv()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestHelpers.SearchPagesAsync(i => TMDbClient.AccountGetRatedTvShowsAsync(i));

        var tvShows = await TMDbClient.AccountGetRatedTvShowsAsync();
        var tvShow = tvShows.Results.Single(s => s.Id == IdHelper.BigBangTheory);

        await Verify(tvShow);
    }

    /// <summary>
    /// Tests that retrieving rated TV episodes returns expected results and supports pagination.
    /// </summary>
    [Fact]
    public async Task TestAccountGetRatedTvEpisodes()
    {
        // TODO: Error in TMDb: https://www.themoviedb.org/talk/557f1af49251410a2c002480
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestHelpers.SearchPagesAsync(i => TMDbClient.AccountGetRatedTvShowEpisodesAsync(i));

        var tvEpisodes = await TMDbClient.AccountGetRatedTvShowEpisodesAsync();

        // Account may or may not have rated episodes
        Assert.NotNull(tvEpisodes);
        if (tvEpisodes.Results.Count > 0)
        {
            var tvEpisode = tvEpisodes.Results.First();
            Assert.True(tvEpisode.Id > 0);
        }
    }

    /// <summary>
    /// Tests that marking and unmarking a TV show as favorite works correctly.
    /// </summary>
    [Fact]
    public async Task TestAccountChangeTvFavoriteStatusAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Ensure that the test movie is not marked as favorite before we start the test
        if (await DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho))
        {
            Assert.True(await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false));
        }
        if (await DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho))
        {
            throw new Exception($"Test tv show '{IdHelper.DoctorWho}' was already marked as favorite. Unable to perform test correctly");
        }
        // Try to mark is as a favorite
        Assert.True(await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.DoctorWho, true));

        // Check if it worked
        Assert.True(await DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho));

        // Try to un-mark is as a favorite
        Assert.True(await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false));

        // Check if it worked
        Assert.False(await DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho));
    }

    /// <summary>
    /// Tests that marking and unmarking a movie as favorite works correctly.
    /// </summary>
    [Fact]
    public async Task TestAccountChangeMovieFavoriteStatusAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Ensure that the test movie is not marked as favorite before we start the test
        if (await DoesFavoriteListContainSpecificMovie(IdHelper.Terminator))
        {
            Assert.True(await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.Terminator, false));
        }
        if (await DoesFavoriteListContainSpecificMovie(IdHelper.Terminator))
        {
            throw new Exception($"Test movie '{IdHelper.Terminator}' was already marked as favorite. Unable to perform test correctly");
        }
        // Try to mark is as a favorite
        Assert.True(await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.Terminator, true));

        // Check if it worked
        Assert.True(await DoesFavoriteListContainSpecificMovie(IdHelper.Terminator));

        // Try to un-mark is as a favorite
        Assert.True(await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.Terminator, false));

        // Check if it worked
        Assert.False(await DoesFavoriteListContainSpecificMovie(IdHelper.Terminator));
    }

    /// <summary>
    /// Tests that adding and removing a TV show from the watchlist works correctly.
    /// </summary>
    [Fact]
    public async Task TestAccountChangeTvWatchlistStatusAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Ensure that the test movie is not marked as favorite before we start the test
        if (await DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho))
        {
            Assert.True(await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false));
        }
        if (await DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho))
        {
            throw new Exception($"Test tv show '{IdHelper.DoctorWho}' was already on watchlist. Unable to perform test correctly");
        }
        // Try to add an item to the watchlist
        Assert.True(await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.DoctorWho, true));

        // Check if it worked
        Assert.True(await DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho));

        // Try to remove item from watchlist
        Assert.True(await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false));

        // Check if it worked
        Assert.False(await DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho));
    }

    /// <summary>
    /// Tests that adding and removing a movie from the watchlist works correctly.
    /// </summary>
    [Fact]
    public async Task TestAccountChangeMovieWatchlistStatusAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Ensure that the test movie is not marked as favorite before we start the test
        if (await DoesWatchListContainSpecificMovie(IdHelper.Terminator))
        {
            Assert.True(await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.Terminator, false));
        }
        if (await DoesWatchListContainSpecificMovie(IdHelper.Terminator))
        {
            throw new Exception($"Test movie '{IdHelper.Terminator}' was already on watchlist. Unable to perform test correctly");
        }
        // Try to add an item to the watchlist
        Assert.True(await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.Terminator, true));

        // Check if it worked
        Assert.True(await DoesWatchListContainSpecificMovie(IdHelper.Terminator));

        // Try to remove item from watchlist
        Assert.True(await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.Terminator, false));

        // Check if it worked
        Assert.False(await DoesWatchListContainSpecificMovie(IdHelper.Terminator));
    }
    private async Task<bool> DoesFavoriteListContainSpecificTvShow(int tvId)
    {
        return await DoesListContainSpecificMovie(tvId, async page => (await TMDbClient.AccountGetFavoriteTvAsync(page)).Results.Select(s => s.Id));
    }
    private async Task<bool> DoesWatchListContainSpecificTvShow(int tvId)
    {
        return await DoesListContainSpecificMovie(tvId, async page => (await TMDbClient.AccountGetTvWatchlistAsync(page)).Results.Select(s => s.Id));
    }
    private async Task<bool> DoesFavoriteListContainSpecificMovie(int movieId)
    {
        return await DoesListContainSpecificMovie(movieId, async page => (await TMDbClient.AccountGetFavoriteMoviesAsync(page)).Results.Select(s => s.Id));
    }
    private async Task<bool> DoesWatchListContainSpecificMovie(int movieId)
    {
        return await DoesListContainSpecificMovie(movieId, async page => (await TMDbClient.AccountGetMovieWatchlistAsync(page)).Results.Select(s => s.Id));
    }
    private async Task<bool> DoesListContainSpecificMovie(int movieId, Func<int, Task<IEnumerable<int>>> listGetter)
    {
        var page = 1;
        var originalList = (await listGetter(page)).ToList();
        while (originalList is not null && originalList.Count != 0)
        {
            // Check if the current result page contains the relevant movie
            if (originalList.Contains(movieId))
            {
                return true;
            }
            // See if there is an other page we could try, if not the test passes
            originalList = originalList.Count != 0 ? (await listGetter(++page)).ToList() : null;
        }
        return false;
    }
}
