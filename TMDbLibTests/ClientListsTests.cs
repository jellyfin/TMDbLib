using System;
using System.Linq;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Movies;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientListsTests : TestBase
    {
        private readonly TestConfig _config;
        private const string TestListId = "528349d419c2954bd21ca0a8";

        public ClientListsTests()
        {
            _config = new TestConfig();
        }

        [Fact]
        public void TestList()
        {
            // Get list
            List list = _config.Client.GetListAsync(TestListId).Result;

            Assert.NotNull(list);
            Assert.Equal(TestListId, list.Id);
            Assert.Equal(list.ItemCount, list.Items.Count);

            foreach (MovieResult movieResult in list.Items)
            {
                Assert.NotNull(movieResult);

                // Ensure all movies point to this list
                int page = 1;
                SearchContainer<ListResult> movieLists = _config.Client.GetMovieListsAsync(movieResult.Id).Result;
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
                        movieLists = _config.Client.GetMovieListsAsync(movieResult.Id, ++page).Result;
                    else
                        throw new Exception($"Movie '{movieResult.Title}' was not linked to the test list");
                }
            }
        }
        
        [Fact]
        public void TestListIsMoviePresentFailure()
        {
            Assert.False(_config.Client.GetListIsMoviePresentAsync(TestListId, IdHelper.Terminator).Result);
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);

            // Clear list
            Assert.True(_config.Client.ListClearAsync(TestListId).Result);

            // Verify Avatar is not present
            Assert.False(_config.Client.GetListIsMoviePresentAsync(TestListId, IdHelper.Avatar).Result);

            // Add Avatar
            Assert.True(_config.Client.ListAddMovieAsync(TestListId, IdHelper.Avatar).Result);

            // Verify Avatar is present
            Assert.True(_config.Client.GetListIsMoviePresentAsync(TestListId, IdHelper.Avatar).Result);
        }

        [Fact]
        public void TestListCreateAndDelete()
        {
            const string listName = "Test List 123";

            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            string newListId = _config.Client.ListCreateAsync(listName).Result;

            Assert.False(string.IsNullOrWhiteSpace(newListId));

            List newlyAddedList = _config.Client.GetListAsync(newListId).Result;
            Assert.NotNull(newlyAddedList);
            Assert.Equal(listName, newlyAddedList.Name);
            Assert.Equal("", newlyAddedList.Description); // "" is the default value
            Assert.Equal("en", newlyAddedList.Iso_639_1); // en is the default value
            Assert.Equal(0, newlyAddedList.ItemCount);
            Assert.Equal(0, newlyAddedList.Items.Count);
            Assert.False(string.IsNullOrWhiteSpace(newlyAddedList.CreatedBy));

            Assert.True(_config.Client.ListDeleteAsync(newListId).Result);
        }

        [Fact]
        public void TestListDeleteFailure()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);

            // Try removing a list with an incorrect id
            Assert.False(_config.Client.ListDeleteAsync("bla").Result);
        }

        [Fact]
        public void TestListAddAndRemoveMovie()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);

            // Add a new movie to the list
            Assert.True(_config.Client.ListAddMovieAsync(TestListId, IdHelper.EvanAlmighty).Result);

            // Try again, this time it should fail since the list already contains this movie
            Assert.False(_config.Client.ListAddMovieAsync(TestListId, IdHelper.EvanAlmighty).Result);

            // Get list and check if the item was added
            List listAfterAdd = _config.Client.GetListAsync(TestListId).Result;
            Assert.True(listAfterAdd.Items.Any(m => m.Id == IdHelper.EvanAlmighty));

            // Remove the previously added movie from the list
            Assert.True(_config.Client.ListRemoveMovieAsync(TestListId, IdHelper.EvanAlmighty).Result);

            // Get list and check if the item was removed
            List listAfterRemove = _config.Client.GetListAsync(TestListId).Result;
            Assert.False(listAfterRemove.Items.Any(m => m.Id == IdHelper.EvanAlmighty));
        }

        [Fact]
        public void TestListClear()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);

            // Add a new movie to the list
            Assert.True(_config.Client.ListAddMovieAsync(TestListId, IdHelper.MadMaxFuryRoad).Result);

            // Get list and check if the item was added
            List listAfterAdd = _config.Client.GetListAsync(TestListId).Result;
            Assert.True(listAfterAdd.Items.Any(m => m.Id == IdHelper.MadMaxFuryRoad));

            // Clear the list
            Assert.True(_config.Client.ListClearAsync(TestListId).Result);

            // Get list and check that all items were removed
            List listAfterRemove = _config.Client.GetListAsync(TestListId).Result;
            Assert.False(listAfterRemove.Items.Any());
        }
    }
}
