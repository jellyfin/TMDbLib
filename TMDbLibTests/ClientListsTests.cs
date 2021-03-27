using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientListsTests : TestBase
    {
        private const string TestListId = "528349d419c2954bd21ca0a8";

        [Fact]
        public async Task TestListAsync()
        {
            // Get list
            GenericList list = await Config.Client.GetListAsync(TestListId);

            Assert.NotNull(list);
            Assert.Equal(TestListId, list.Id);
            Assert.Equal(list.ItemCount, list.Items.Count);

            foreach (SearchMovie movieResult in list.Items)
            {
                Assert.NotNull(movieResult);

                // Ensure all movies point to this list
                int page = 1;
                SearchContainer<ListResult> movieLists = await Config.Client.GetMovieListsAsync(movieResult.Id);
                while (movieLists != null)
                {
                    // Check if the current result page contains the relevant list
                    if (movieLists.Results.Any(s => s.Id == TestListId))
                    {
                        movieLists = null;
                        continue;
                    }

                    // See if there is an other page we could try, if not the test fails
                    if (movieLists.Page < movieLists.TotalPages)
                        movieLists = await Config.Client.GetMovieListsAsync(movieResult.Id, ++page);
                    else
                        throw new Exception($"Movie '{movieResult.Title}' was not linked to the test list");
                }
            }
        }

        [Fact]
        public async Task TestListMissingAsync()
        {
            GenericList list = await Config.Client.GetListAsync(IdHelper.MissingID.ToString());

            Assert.Null(list);
        }

        [Fact]
        public async Task TestListIsMoviePresentFailureAsync()
        {
            Assert.False(await Config.Client.GetListIsMoviePresentAsync(TestListId, IdHelper.Terminator));
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);

            // Clear list
            Assert.True(await Config.Client.ListClearAsync(TestListId));

            // Verify Avatar is not present
            Assert.False(await Config.Client.GetListIsMoviePresentAsync(TestListId, IdHelper.Avatar));

            // Add Avatar
            Assert.True(await Config.Client.ListAddMovieAsync(TestListId, IdHelper.Avatar));

            // Verify Avatar is present
            Assert.True(await Config.Client.GetListIsMoviePresentAsync(TestListId, IdHelper.Avatar));
        }

        [Fact]
        public async Task TestListCreateAndDeleteAsync()
        {
            const string listName = "Test List 123";

            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);
            string newListId = await Config.Client.ListCreateAsync(listName);

            Assert.False(string.IsNullOrWhiteSpace(newListId));

            GenericList newlyAddedList = await Config.Client.GetListAsync(newListId);
            Assert.NotNull(newlyAddedList);
            Assert.Equal(listName, newlyAddedList.Name);
            Assert.Equal("", newlyAddedList.Description); // "" is the default value
            Assert.Equal("en", newlyAddedList.Iso_639_1); // en is the default value
            Assert.Equal(0, newlyAddedList.ItemCount);
            Assert.Empty(newlyAddedList.Items);
            Assert.False(string.IsNullOrWhiteSpace(newlyAddedList.CreatedBy));

            Assert.True(await Config.Client.ListDeleteAsync(newListId));
        }

        [Fact]
        public async Task TestListDeleteFailureAsync()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);

            // Try removing a list with an incorrect id
            Assert.False(await Config.Client.ListDeleteAsync("bla"));
        }

        [Fact]
        public async Task TestListAddAndRemoveMovieAsync()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);

            // Add a new movie to the list
            Assert.True(await Config.Client.ListAddMovieAsync(TestListId, IdHelper.EvanAlmighty));

            // Try again, this time it should fail since the list already contains this movie
            Assert.False(await Config.Client.ListAddMovieAsync(TestListId, IdHelper.EvanAlmighty));

            // Get list and check if the item was added
            GenericList listAfterAdd = await Config.Client.GetListAsync(TestListId);
            Assert.Contains(listAfterAdd.Items, m => m.Id == IdHelper.EvanAlmighty);

            // Remove the previously added movie from the list
            Assert.True(await Config.Client.ListRemoveMovieAsync(TestListId, IdHelper.EvanAlmighty));

            // Get list and check if the item was removed
            GenericList listAfterRemove = await Config.Client.GetListAsync(TestListId);
            Assert.DoesNotContain(listAfterRemove.Items, m => m.Id == IdHelper.EvanAlmighty);
        }

        [Fact]
        public async Task TestListClearAsync()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);

            // Add a new movie to the list
            Assert.True(await Config.Client.ListAddMovieAsync(TestListId, IdHelper.MadMaxFuryRoad));

            // Get list and check if the item was added
            GenericList listAfterAdd = await Config.Client.GetListAsync(TestListId);
            Assert.Contains(listAfterAdd.Items, m => m.Id == IdHelper.MadMaxFuryRoad);

            // Clear the list
            Assert.True(await Config.Client.ListClearAsync(TestListId));

            // Get list and check that all items were removed
            GenericList listAfterRemove = await Config.Client.GetListAsync(TestListId);
            Assert.False(listAfterRemove.Items.Any());
        }
    }
}