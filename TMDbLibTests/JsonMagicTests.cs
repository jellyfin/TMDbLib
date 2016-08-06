using System.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests
{
    public class JsonMagicTests : TestBase
    {
        /// <summary>
        /// Tests the KnownForConverter
        /// </summary>
        [Fact]
        public void TestJsonKnownForConverter()
        {
            SearchContainer<SearchPerson> result = Config.Client.SearchPersonAsync("Willis").Result;

            Assert.NotNull(result);
            Assert.NotNull(result.Results);

            var knownForList = result.Results.SelectMany(s => s.KnownFor).ToList();
            Assert.True(knownForList.Any());

            Assert.Contains(knownForList, item => item.MediaType == MediaType.Tv && item is KnownForTv);
            Assert.Contains(knownForList, item => item.MediaType == MediaType.Movie && item is KnownForMovie);
        }

        /// <summary>
        /// Tests the TaggedImageConverter
        /// </summary>
        [Fact]
        public void TestJsonTaggedImageConverter()
        {
            // Get images
            SearchContainerWithId<TaggedImage> result = Config.Client.GetPersonTaggedImagesAsync(IdHelper.HughLaurie, 1).Result;

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Equal(IdHelper.HughLaurie, result.Id);

            Assert.Contains(result.Results, item => item.MediaType == MediaType.Tv && item.Media is SearchTv);
            Assert.Contains(result.Results, item => item.MediaType == MediaType.Movie && item.Media is SearchMovie);
        }

        /// <summary>
        /// Tests the SearchBaseConverter
        /// </summary>
        [Fact]
        public void TestSearchBaseConverter()
        {
            TestHelpers.SearchPages(i => Config.Client.SearchMultiAsync("Rock", i).Result);
            SearchContainer<SearchBase> result = Config.Client.SearchMultiAsync("Rock").Result;

            Assert.NotNull(result);
            Assert.NotNull(result.Results);

            Assert.Contains(result.Results, item => item.MediaType == MediaType.Tv && item is SearchTv);
            Assert.Contains(result.Results, item => item.MediaType == MediaType.Movie && item is SearchMovie);
            Assert.Contains(result.Results, item => item.MediaType == MediaType.Person && item is SearchPerson);
        }
    }
}