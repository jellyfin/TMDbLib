using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class TaggedImageConverterTest : TestBase
    {
        /// <summary>
        /// Tests the TaggedImageConverter
        /// </summary>
        [Fact]
        public void TestJsonTaggedImageConverter()
        {
            // Get images
            SearchContainerWithId<TaggedImage> result = Config.Client.GetPersonTaggedImagesAsync(IdHelper.HughLaurie, 1).Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Equal(IdHelper.HughLaurie, result.Id);

            Assert.Contains(result.Results, item => item.MediaType == MediaType.Tv && item.Media is SearchTv);
            Assert.Contains(result.Results, item => item.MediaType == MediaType.Movie && item.Media is SearchMovie);
        }
    }
}