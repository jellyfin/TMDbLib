using System.Threading.Tasks;
using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Converters;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class TaggedImageConverterTest : TestBase
    {
        [Fact]
        public void TaggedImageConverter_Movie()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new TaggedImageConverter());

            SearchMovie originalMedia = new SearchMovie { OriginalTitle = "Hello world" };

            TaggedImage original = new TaggedImage
            {
                MediaType = originalMedia.MediaType,
                Media = originalMedia
            };

            string json = JsonConvert.SerializeObject(original, settings);
            TaggedImage result = JsonConvert.DeserializeObject<TaggedImage>(json, settings);

            Assert.NotNull(result);
            Assert.NotNull(result.Media);
            Assert.IsType<SearchMovie>(result.Media);
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.MediaType, result.Media.MediaType);

            SearchMovie resultMedia = (SearchMovie)result.Media;
            Assert.Equal(originalMedia.OriginalTitle, resultMedia.OriginalTitle);
        }

        [Fact]
        public void TaggedImageConverter_Tv()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new TaggedImageConverter());

            SearchTv originalMedia = new SearchTv { OriginalName = "Hello world" };

            TaggedImage original = new TaggedImage();
            original.MediaType = MediaType.Tv;
            original.Media = originalMedia;

            string json = JsonConvert.SerializeObject(original, settings);
            TaggedImage result = JsonConvert.DeserializeObject<TaggedImage>(json, settings);

            Assert.NotNull(result);
            Assert.NotNull(result.Media);
            Assert.IsType<SearchTv>(result.Media);
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.MediaType, result.Media.MediaType);

            SearchTv resultMedia = (SearchTv)result.Media;
            Assert.Equal(originalMedia.OriginalName, resultMedia.OriginalName);
        }

        /// <summary>
        /// Tests the TaggedImageConverter
        /// </summary>
        [Fact]
        public async Task TestJsonTaggedImageConverter()
        {
            // Get images
            SearchContainerWithId<TaggedImage> result = await TMDbClient.GetPersonTaggedImagesAsync(IdHelper.HughLaurie, 1);

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Equal(IdHelper.HughLaurie, result.Id);

            Assert.All(result.Results, item =>
            {
                if (item.MediaType == MediaType.Tv)
                    Assert.IsType<SearchTv>(item.Media);
            });
            Assert.All(result.Results, item =>
            {
                if (item.MediaType == MediaType.Movie)
                    Assert.IsType<SearchMovie>(item.Media);
            });
        }
    }
}