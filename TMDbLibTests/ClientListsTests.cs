using System;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using System.Globalization;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb lists functionality.
/// </summary>
[Collection(nameof(ListFixturesCollection))]
public class ClientListsTests : TestBase
{
    private const string TestListId = "528349d419c2954bd21ca0a8";
    private const string EphemeralListPrefix = "TestListTMDbLib-";

    /// <summary>
    /// Tests that a list can be retrieved by ID.
    /// </summary>
    [Fact]
    public async Task TestGetListAsync()
    {
        // Get list
        var list = await TMDbClient.GetListAsync(TestListId);

        await Verify(list);
    }

    /// <summary>
    /// Tests that movie lists can be retrieved for a specific movie.
    /// </summary>
    [Fact]
    public async Task TestListAsync()
    {
        var movieLists = await TMDbClient.GetMovieListsAsync(IdHelper.Avatar);

        Assert.NotEmpty(movieLists.Results);
        Assert.All(movieLists.Results, x => Assert.Equal(MediaType.Movie, x.ListType));
    }

    /// <summary>
    /// Verifies that retrieving a non-existent list returns null.
    /// </summary>
    [Fact]
    public async Task TestListMissingAsync()
    {
        var list = await TMDbClient.GetListAsync(IdHelper.MissingID.ToString(CultureInfo.InvariantCulture));

        Assert.Null(list);
    }

    /// <summary>
    /// Tests that a list can be created, have movies added and removed, be cleared, and be deleted.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestListCreateAddClearAndDeleteAsync()
    {
        var listName = EphemeralListPrefix + DateTime.UtcNow.ToString("O");

        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        var listId = await TMDbClient.ListCreateAsync(listName);

        Assert.False(string.IsNullOrWhiteSpace(listId));

        var newlyAddedList = await TMDbClient.GetListAsync(listId);

        Assert.NotNull(newlyAddedList);
        Assert.Equal(listName, newlyAddedList.Name);
        Assert.Empty(newlyAddedList.Items);

        // Add a movie
        await TMDbClient.ListAddMovieAsync(listId, IdHelper.Avatar);
        await TMDbClient.ListAddMovieAsync(listId, IdHelper.AGoodDayToDieHard);

        Assert.True(await TMDbClient.GetListIsMoviePresentAsync(listId, IdHelper.Avatar));

        // Remove a movie
        await TMDbClient.ListRemoveMovieAsync(listId, IdHelper.Avatar);

        Assert.False(await TMDbClient.GetListIsMoviePresentAsync(listId, IdHelper.Avatar));

        // Clear the list
        await TMDbClient.ListClearAsync(listId);

        Assert.False(await TMDbClient.GetListIsMoviePresentAsync(listId, IdHelper.AGoodDayToDieHard));

        // Delete the list
        Assert.True(await TMDbClient.ListDeleteAsync(listId));
    }

    /// <summary>
    /// Verifies that attempting to delete a list with an invalid ID fails gracefully.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestListDeleteFailureAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Try removing a list with an incorrect id
        // API may return false or throw an exception for invalid IDs
        try
        {
            var result = await TMDbClient.ListDeleteAsync("invalid_id");
            Assert.False(result);
        }
        catch (TMDbLib.Objects.Exceptions.GeneralHttpException)
        {
            // Expected - API now throws for invalid IDs
        }
        catch (NullReferenceException)
        {
            // Expected - API may return null response for invalid IDs
        }
    }

    private class ListCleanupFixture : IDisposable
    {
        public void Dispose()
        {
            var config = new TestConfig();
            var client = config.Client;

            client.SetSessionInformationAsync(config.UserSessionId, SessionType.UserSession).GetAwaiter().GetResult();

            // Yes, this is only the first page, but that's fine.
            // Eventually we'll delete all remaining lists
            var lists = client.AccountGetListsAsync().GetAwaiter().GetResult();

            foreach (var list in lists.Results.Where(s => s.Name.StartsWith(EphemeralListPrefix, StringComparison.Ordinal)))
            {
                client.ListDeleteAsync(list.Id.ToString(CultureInfo.InvariantCulture)).GetAwaiter().GetResult();
            }
        }
    }

    /// <summary>
    /// Collection definition for list cleanup fixtures.
    /// </summary>
    [CollectionDefinition(nameof(ListFixturesCollection))]
    public class ListFixturesCollection : ICollectionFixture<ListCleanupFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
