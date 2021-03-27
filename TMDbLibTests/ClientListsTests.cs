using System;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Movies;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    [Collection(nameof(ListFixturesCollection))]
    public class ClientListsTests : TestBase
    {
        private const string TestListId = "528349d419c2954bd21ca0a8";
        private const string EphemeralListPrefix = "TestListTMDbLib-";

        [Fact]
        public async Task TestGetListAsync()
        {
            // Get list
            GenericList list = await TMDbClient.GetListAsync(TestListId);

            await Verify(list);
        }

        [Fact]
        public async Task TestListAsync()
        {
            SearchContainer<ListResult> movieLists = await TMDbClient.GetMovieListsAsync(IdHelper.Avatar);

            Assert.NotEmpty(movieLists.Results);
            Assert.All(movieLists.Results, x => Assert.Equal(MediaType.Movie, x.ListType));
        }

        [Fact]
        public async Task TestListMissingAsync()
        {
            GenericList list = await TMDbClient.GetListAsync(IdHelper.MissingID.ToString());

            Assert.Null(list);
        }

        [Fact]
        public async Task TestListCreateAddClearAndDeleteAsync()
        {
            string listName = EphemeralListPrefix + DateTime.UtcNow.ToString("O");

            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            string listId = await TMDbClient.ListCreateAsync(listName);

            Assert.False(string.IsNullOrWhiteSpace(listId));

            GenericList newlyAddedList = await TMDbClient.GetListAsync(listId);

            await Verify(newlyAddedList, settings => settings.IgnoreProperty<GenericList>(x => x.Id, x => x.Name));

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

        [Fact]
        public async Task TestListDeleteFailureAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Try removing a list with an incorrect id
            Assert.False(await TMDbClient.ListDeleteAsync("invalid_id"));
        }

        private class ListCleanupFixture : IDisposable
        {
            public void Dispose()
            {
                TestConfig config = new TestConfig();
                TMDbClient client = config.Client;

                client.SetSessionInformationAsync(config.UserSessionId, SessionType.UserSession).GetAwaiter().GetResult();

                // Yes, this is only the first page, but that's fine.
                // Eventually we'll delete all remaining lists
                SearchContainer<AccountList> lists = client.AccountGetListsAsync().GetAwaiter().GetResult();

                foreach (AccountList list in lists.Results.Where(s => s.Name.StartsWith(EphemeralListPrefix)))
                {
                    client.ListDeleteAsync(list.Id.ToString()).GetAwaiter().GetResult();
                }
            }
        }

        [CollectionDefinition(nameof(ListFixturesCollection))]
        public class ListFixturesCollection : ICollectionFixture<ListCleanupFixture>
        {
            // This class has no code, and is never created. Its purpose is simply
            // to be the place to apply [CollectionDefinition] and all the
            // ICollectionFixture<> interfaces.
        }
    }
}