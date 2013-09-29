using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Movies;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientListsTests
    {
        private const int Avatar = 19995;
        private TestConfig _config;

        [TestInitialize]
        public void InitTest()
        {
            _config = new TestConfig();
        }

        [TestMethod]
        public void TestList()
        {
            // Get a list Id
            string listId = _config.Client.GetMovieLists(Avatar).Results.First().Id;

            // Get list
            List list = _config.Client.GetList(listId);

            Assert.IsNotNull(list);
            Assert.AreEqual(listId, list.Id);
            Assert.AreEqual(list.ItemCount, list.Items.Count);

            foreach (MovieResult movieResult in list.Items)
            {
                Assert.IsNotNull(movieResult);

                // Ensure all movies point to this list
                List<ListResult> movieLists = _config.Client.GetMovieLists(movieResult.Id).Results;
                Assert.IsTrue(movieLists.Any(s => s.Id == listId));
            }
        }
    }
}
