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
            GenericList list = await TMDbClient.GetListAsync(TestListId);

            Assert.NotNull(list);
            Assert.Equal(TestListId, list.Id);
            Assert.Equal(list.ItemCount, list.Items.Count);

            foreach (SearchMovie movieResult in list.Items)
            {
                Assert.NotNull(movieResult);

                // Ensure all movies point to this list
                int page = 1;
                SearchContainer<ListResult> movieLists = await TMDbClient.GetMovieListsAsync(movieResult.Id);
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
                        movieLists = await TMDbClient.GetMovieListsAsync(movieResult.Id, ++page);
                    else
                        throw new Exception($"Movie '{movieResult.Title}' was not linked to the test list");
                }
            }
        }

        [Fact]
        public async Task TestListMissingAsync()
        {
            GenericList list = await TMDbClient.GetListAsync(IdHelper.MissingID.ToString());

            Assert.Null(list);
        }

        [Fact]
        public async Task TestListIsMoviePresentFailureAsync()
        {
            Assert.False(await TMDbClient.GetListIsMoviePresentAsync(TestListId, IdHelper.Terminator));
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Clear list
            Assert.True(await TMDbClient.ListClearAsync(TestListId));

            // Verify Avatar is not present
            Assert.False(await TMDbClient.GetListIsMoviePresentAsync(TestListId, IdHelper.Avatar));

            // Add Avatar
            Assert.True(await TMDbClient.ListAddMovieAsync(TestListId, IdHelper.Avatar));

            // Verify Avatar is present
            Assert.True(await TMDbClient.GetListIsMoviePresentAsync(TestListId, IdHelper.Avatar));
        }

        [Fact]
        public async Task TestListCreateAndDeleteAsync()
        {
            const string listName = "Test List 123";

            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            string newListId = await TMDbClient.ListCreateAsync(listName);

            Assert.False(string.IsNullOrWhiteSpace(newListId));

            GenericList newlyAddedList = await TMDbClient.GetListAsync(newListId);
            Assert.NotNull(newlyAddedList);
            Assert.Equal(listName, newlyAddedList.Name);
            Assert.Equal("", newlyAddedList.Description); // "" is the default value
            Assert.Equal("en", newlyAddedList.Iso_639_1); // en is the default value
            Assert.Equal(0, newlyAddedList.ItemCount);
            Assert.Empty(newlyAddedList.Items);
            Assert.False(string.IsNullOrWhiteSpace(newlyAddedList.CreatedBy));

            Assert.True(await TMDbClient.ListDeleteAsync(newListId));
        }

        [Fact]
        public async Task TestListDeleteFailureAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Try removing a list with an incorrect id
            Assert.False(await TMDbClient.ListDeleteAsync("bla"));
        }

        [Fact]
        public async Task TestListAddAndRemoveMovieAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Add a new movie to the list
            Assert.True(await TMDbClient.ListAddMovieAsync(TestListId, IdHelper.EvanAlmighty));

            // Try again, this time it should fail since the list already contains this movie
            Assert.False(await TMDbClient.ListAddMovieAsync(TestListId, IdHelper.EvanAlmighty));

            // Get list and check if the item was added
            GenericList listAfterAdd = await TMDbClient.GetListAsync(TestListId);
            Assert.Contains(listAfterAdd.Items, m => m.Id == IdHelper.EvanAlmighty);

            // Remove the previously added movie from the list
            Assert.True(await TMDbClient.ListRemoveMovieAsync(TestListId, IdHelper.EvanAlmighty));

            // Get list and check if the item was removed
            GenericList listAfterRemove = await TMDbClient.GetListAsync(TestListId);
            Assert.DoesNotContain(listAfterRemove.Items, m => m.Id == IdHelper.EvanAlmighty);
        }

        [Fact]
        public async Task TestListClearAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Add a new movie to the list
            Assert.True(await TMDbClient.ListAddMovieAsync(TestListId, IdHelper.MadMaxFuryRoad));

            // Get list and check if the item was added
            GenericList listAfterAdd = await TMDbClient.GetListAsync(TestListId);
            Assert.Contains(listAfterAdd.Items, m => m.Id == IdHelper.MadMaxFuryRoad);

            // Clear the list
            Assert.True(await TMDbClient.ListClearAsync(TestListId));

            // Get list and check that all items were removed
            GenericList listAfterRemove = await TMDbClient.GetListAsync(TestListId);
            Assert.False(listAfterRemove.Items.Any());
        }
    }
}