using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class TaggedImageConverterTest : TestBase
    {
        [Fact]
        public async Task TaggedImageConverter_Movie()
        {
            SearchMovie originalMedia = new SearchMovie { OriginalTitle = "Hello world" };

            TaggedImage original = new TaggedImage
            {
                MediaType = originalMedia.MediaType,
                Media = originalMedia
            };

            string json = Serializer.SerializeToString(original);
            TaggedImage result = Serializer.DeserializeFromString<TaggedImage>(json);

            Assert.IsType<SearchMovie>(result.Media);
            await Verify(new
            {
                json,
                result
            });
        }

        [Fact]
        public async Task TaggedImageConverter_Tv()
        {
            SearchTv originalMedia = new SearchTv { OriginalName = "Hello world" };

            TaggedImage original = new TaggedImage();
            original.MediaType = MediaType.Tv;
            original.Media = originalMedia;

            string json = Serializer.SerializeToString(original);
            TaggedImage result = Serializer.DeserializeFromString<TaggedImage>(json);

            Assert.IsType<SearchTv>(result.Media);
            await Verify(new
            {
                json,
                result
            });
        }

        /// <summary>
        /// Tests the TaggedImageConverter
        /// </summary>
        [Theory]
        [InlineData(IdHelper.HughLaurie)]   // Has Movie media
        [InlineData(IdHelper.TomHanks)] // Has Episode media
        public async Task TestJsonTaggedImageConverter(int personId)
        {
            // Get images
            SearchContainerWithId<TaggedImage> result = await TMDbClient.GetPersonTaggedImagesAsync(personId, 1);

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.NotEmpty(result.Results);
            Assert.Equal(personId, result.Id);

            Assert.All(result.Results, item =>
            {
                if (item.MediaType == MediaType.Movie)
                    Assert.IsType<SearchMovie>(item.Media);
                else if (item.MediaType == MediaType.Tv)
                    Assert.IsType<SearchTv>(item.Media);
                else if (item.MediaType == MediaType.Episode)
                    Assert.IsType<SearchTvEpisode>(item.Media);
                else
                    Assert.False(true, $"Unexpected type {item.GetType().Name}");
            });
        }
    }
}