using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Movies;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientListsTests
    {
        private const int Avatar = 19995;
        private const int Terminator = 218;
        private const int EvanAlmighty = 2698;
        private const string TestListId = "527fa7f3760ee361f70c8b14";
        private TestConfig _config;

        [TestInitialize]
        public void InitTest()
        {
            _config = new TestConfig();
        }

        [TestMethod]
        public void TestList()
        {
            // Get list
            List list = _config.Client.GetList(TestListId);

            Assert.IsNotNull(list);
            Assert.AreEqual(TestListId, list.Id);
            Assert.AreEqual(list.ItemCount, list.Items.Count);

            foreach (MovieResult movieResult in list.Items)
            {
                Assert.IsNotNull(movieResult);

                // Ensure all movies point to this list
                int page = 1;
                SearchContainer<ListResult> movieLists = _config.Client.GetMovieLists(movieResult.Id);
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
                        movieLists = _config.Client.GetMovieLists(movieResult.Id, ++page);
                    else
                       Assert.Fail("Movie '{0}' was not linked to the test list", movieResult.Title);
                }
            }
        }

        [TestMethod]
        public void TestListIsMoviePresentSuccess()
        {
            Assert.IsTrue(_config.Client.GetListIsMoviePresent(TestListId, Avatar));
        }

        [TestMethod]
        public void TestListIsMoviePresentFailure()
        {
            Assert.IsFalse(_config.Client.GetListIsMoviePresent(TestListId, Terminator));
        }

        [TestMethod]
        public void TestListCreateAndDelete()
        {
            const string listName = "Test List 123";

            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            string newListId = _config.Client.ListCreate(listName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(newListId));

            var newlyAddedList = _config.Client.GetList(newListId);
            Assert.IsNotNull(newlyAddedList);
            Assert.AreEqual(listName, newlyAddedList.Name);
            Assert.AreEqual("", newlyAddedList.Description); // "" is the default value
            Assert.AreEqual("en", newlyAddedList.Iso_639_1); // en is the default value
            Assert.AreEqual(0, newlyAddedList.ItemCount);
            Assert.AreEqual(0, newlyAddedList.Items.Count);
            Assert.IsFalse(string.IsNullOrWhiteSpace(newlyAddedList.CreatedBy));

            Assert.IsTrue(_config.Client.ListDelete(newListId));
            _config.Client.SetSessionInformation(null, SessionType.Unassigned);
        }

        [TestMethod]
        public void TestListDeleteFailure()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            // Try removing a list with an incorrect id
            Assert.IsFalse(_config.Client.ListDelete("bla"));
            _config.Client.SetSessionInformation(null, SessionType.Unassigned);
        }

        [TestMethod]
        public void TestListAddAndRemoveMovie()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            // Add a new movie to the list
            Assert.IsTrue(_config.Client.ListAddMovie(TestListId, EvanAlmighty));

            // Try again, this time it should fail since the list already contains this movie
            Assert.IsFalse(_config.Client.ListAddMovie(TestListId, EvanAlmighty));

            // Get list and check if the item was added
            var listAfterAdd = _config.Client.GetList(TestListId);
            Assert.IsTrue(listAfterAdd.Items.Any(m => m.Id == EvanAlmighty));

            // Remove the previously added movie from the list
            Assert.IsTrue(_config.Client.ListRemoveMovie(TestListId, EvanAlmighty));

            // Get list and check if the item was removed
            var listAfterRemove = _config.Client.GetList(TestListId);
            Assert.IsFalse(listAfterRemove.Items.Any(m => m.Id == EvanAlmighty));
            _config.Client.SetSessionInformation(null, SessionType.Unassigned);
        }
    }
}
