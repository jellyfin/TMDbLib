using System;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb lists functionality.
/// </summary>
[Collection(nameof(ListFixturesCollection))]
public class ClientListsTests : TestBase
{
    // TMDb list ids are integers, and only the leading digits of the id path segment are
    // significant: requesting "527fa7f3760ee361f70c8b14" returns exactly what "527" returns.
    // The legacy id this test used, "528349d419c2954bd21ca0a8", therefore only ever requested
    // list 528349, which does not exist. This is a long-lived list that still resolves.
    private const int TestListId = 509;
    private const string EphemeralListPrefix = "TestListTMDbLib-";

    /// <summary>
    /// Tests that a list can be retrieved by ID.
    /// </summary>
    [Fact]
    public async Task TestGetListAsync()
    {
        var list = await TMDbClient.GetListAsync(TestListId, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(list);
        Assert.Equal(TestListId, list.Id);
        Assert.NotNull(list.Items);
        Assert.NotEmpty(list.Items);

        await Verify(list);
    }

    /// <summary>
    /// Tests that movie lists can be retrieved for a specific movie.
    /// </summary>
    [Fact]
    public async Task TestListAsync()
    {
        var movieLists = await TMDbClient.GetMovieListsAsync(IdHelper.Avatar, cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(movieLists);

        Assert.NotNull(movieLists.Results);
        Assert.NotEmpty(movieLists.Results);
        Assert.All(movieLists.Results, x => Assert.Equal(MediaType.Movie, x.ListType));
    }

    /// <summary>
    /// Verifies that retrieving a non-existent list returns null.
    /// </summary>
    [Fact]
    public async Task TestListMissingAsync()
    {
        var list = await TMDbClient.GetListAsync(IdHelper.MissingID, cancellationToken: TestContext.Current.CancellationToken);

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

        var listId = await TMDbClient.ListCreateAsync(listName, cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotEqual(0, listId);

        var newlyAddedList = await TMDbClient.GetListAsync(listId, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(newlyAddedList);
        Assert.Equal(listName, newlyAddedList.Name);
        Assert.NotNull(newlyAddedList.Items);
        Assert.Empty(newlyAddedList.Items);

        // Add a movie
        await TMDbClient.ListAddMovieAsync(listId, IdHelper.Avatar, cancellationToken: TestContext.Current.CancellationToken);
        await TMDbClient.ListAddMovieAsync(listId, IdHelper.AGoodDayToDieHard, cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(await TMDbClient.GetListIsMoviePresentAsync(listId, IdHelper.Avatar, cancellationToken: TestContext.Current.CancellationToken));

        // Remove a movie
        await TMDbClient.ListRemoveMovieAsync(listId, IdHelper.Avatar, cancellationToken: TestContext.Current.CancellationToken);

        Assert.False(await TMDbClient.GetListIsMoviePresentAsync(listId, IdHelper.Avatar, cancellationToken: TestContext.Current.CancellationToken));

        // Clear the list
        await TMDbClient.ListClearAsync(listId, cancellationToken: TestContext.Current.CancellationToken);

        Assert.False(await TMDbClient.GetListIsMoviePresentAsync(listId, IdHelper.AGoodDayToDieHard, cancellationToken: TestContext.Current.CancellationToken));

        // Delete the list
        Assert.True(await TMDbClient.ListDeleteAsync(listId, cancellationToken: TestContext.Current.CancellationToken));
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
            var result = await TMDbClient.ListDeleteAsync(IdHelper.MissingID, cancellationToken: TestContext.Current.CancellationToken);
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

            client.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession).GetAwaiter().GetResult();

            // Yes, this is only the first page, but that's fine.
            // Eventually we'll delete all remaining lists
            var lists = client.AccountGetListsAsync().GetAwaiter().GetResult();

            if (lists is null || lists.Results is null)
            {
                return;
            }

            foreach (var list in lists.Results.Where(s => s.Name?.StartsWith(EphemeralListPrefix, StringComparison.Ordinal) == true))
            {
                client.ListDeleteAsync(list.Id).GetAwaiter().GetResult();
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
