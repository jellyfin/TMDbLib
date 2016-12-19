using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class SearchBaseConverterTest : TestBase
    {
        /// <summary>
        /// Tests the SearchBaseConverter
        /// </summary>
        [Fact]
        public void TestSearchBaseConverter()
        {
            TestHelpers.SearchPages(i => Config.Client.SearchMultiAsync("Rock", i).Sync());
            SearchContainer<SearchBase> result = Config.Client.SearchMultiAsync("Rock").Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);

            Assert.Contains(result.Results, item => item.MediaType == MediaType.Tv && item is SearchTv);
            Assert.Contains(result.Results, item => item.MediaType == MediaType.Movie && item is SearchMovie);
            Assert.Contains(result.Results, item => item.MediaType == MediaType.Person && item is SearchPerson);
        }
    }
}