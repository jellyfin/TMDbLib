using System;
using System.Linq;
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
        public void TestList()
        {
            // Get list
            GenericList list = Config.Client.GetListAsync(TestListId).Result;

            Assert.NotNull(list);
            Assert.Equal(TestListId, list.Id);
            Assert.Equal(list.ItemCount, list.Items.Count);

            foreach (SearchMovie movieResult in list.Items)
            {
                Assert.NotNull(movieResult);

                // Ensure all movies point to this list
                int page = 1;
                SearchContainer<ListResult> movieLists = Config.Client.GetMovieListsAsync(movieResult.Id).Result;

                ListResult listItem = movieLists.Results.First();
                Assert.NotEmpty(listItem.Description);
                Assert.NotEmpty(listItem.Iso_639_1);
                Assert.NotEmpty(listItem.Id);
                Assert.NotEmpty(listItem.Name);

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
                        movieLists = Config.Client.GetMovieListsAsync(movieResult.Id, ++page).Result;
                    else
                        throw new Exception($"Movie '{movieResult.Title}' was not linked to the test list");
                }
            }
        }

        [Fact]
        public void TestListIsMoviePresentFailure()
        {
            Assert.False(Config.Client.GetListIsMoviePresentAsync(TestListId, IdHelper.Terminator).Result);
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Clear list
            Assert.True(Config.Client.ListClearAsync(TestListId).Result);

            // Verify Avatar is not present
            Assert.False(Config.Client.GetListIsMoviePresentAsync(TestListId, IdHelper.Avatar).Result);

            // Add Avatar
            Assert.True(Config.Client.ListAddMovieAsync(TestListId, IdHelper.Avatar).Result);

            // Verify Avatar is present
            Assert.True(Config.Client.GetListIsMoviePresentAsync(TestListId, IdHelper.Avatar).Result);
        }

        [Fact]
        public void TestListCreateAndDelete()
        {
            const string listName = "Test List 123";

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            string newListId = Config.Client.ListCreateAsync(listName).Result;

            Assert.False(string.IsNullOrWhiteSpace(newListId));

            GenericList newlyAddedList = Config.Client.GetListAsync(newListId).Result;
            Assert.NotNull(newlyAddedList);
            Assert.Equal(listName, newlyAddedList.Name);
            Assert.Equal("", newlyAddedList.Description); // "" is the default value
            Assert.Equal("en", newlyAddedList.Iso_639_1); // en is the default value
            Assert.Equal(0, newlyAddedList.ItemCount);
            Assert.Equal(0, newlyAddedList.Items.Count);
            Assert.False(string.IsNullOrWhiteSpace(newlyAddedList.CreatedBy));

            Assert.True(Config.Client.ListDeleteAsync(newListId).Result);
        }

        [Fact]
        public void TestListDeleteFailure()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Try removing a list with an incorrect id
            Assert.False(Config.Client.ListDeleteAsync("bla").Result);
        }

        [Fact]
        public void TestListAddAndRemoveMovie()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Add a new movie to the list
            Assert.True(Config.Client.ListAddMovieAsync(TestListId, IdHelper.EvanAlmighty).Result);

            // Try again, this time it should fail since the list already contains this movie
            Assert.False(Config.Client.ListAddMovieAsync(TestListId, IdHelper.EvanAlmighty).Result);

            // Get list and check if the item was added
            GenericList listAfterAdd = Config.Client.GetListAsync(TestListId).Result;
            Assert.True(listAfterAdd.Items.Any(m => m.Id == IdHelper.EvanAlmighty));

            // Remove the previously added movie from the list
            Assert.True(Config.Client.ListRemoveMovieAsync(TestListId, IdHelper.EvanAlmighty).Result);

            // Get list and check if the item was removed
            GenericList listAfterRemove = Config.Client.GetListAsync(TestListId).Result;
            Assert.False(listAfterRemove.Items.Any(m => m.Id == IdHelper.EvanAlmighty));
        }

        [Fact]
        public void TestListClear()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Add a new movie to the list
            Assert.True(Config.Client.ListAddMovieAsync(TestListId, IdHelper.MadMaxFuryRoad).Result);

            // Get list and check if the item was added
            GenericList listAfterAdd = Config.Client.GetListAsync(TestListId).Result;
            Assert.True(listAfterAdd.Items.Any(m => m.Id == IdHelper.MadMaxFuryRoad));

            // Clear the list
            Assert.True(Config.Client.ListClearAsync(TestListId).Result);

            // Get list and check that all items were removed
            GenericList listAfterRemove = Config.Client.GetListAsync(TestListId).Result;
            Assert.False(listAfterRemove.Items.Any());
        }
    }
}
