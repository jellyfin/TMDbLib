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

            TaggedImage original = new TaggedImage();
            original.MediaType = MediaType.Movie;
            original.Media = originalMedia;

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
        public void TestJsonTaggedImageConverter()
        {
            // Ignore fields not set
            IgnoreMissingCSharp("_id / _id", "adult / adult", "backdrop_path / backdrop_path", "first_air_date / first_air_date", "genre_ids / genre_ids", "name / name", "origin_country / origin_country", "original_language / original_language", "original_name / original_name", "original_title / original_title", "overview / overview", "poster_path / poster_path", "release_date / release_date", "title / title", "video / video", "vote_average / vote_average", "vote_count / vote_count");
            IgnoreMissingJson(" / media_type");

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