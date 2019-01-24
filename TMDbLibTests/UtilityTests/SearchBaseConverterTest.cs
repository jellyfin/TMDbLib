using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Converters;
using TMDbLibTests.Helpers;
using TMDbLibTests.TestFramework;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class SearchBaseConverterTest : TestBase
    {
        public SearchBaseConverterTest(TestConfig testConfig) : base(testConfig)
        {
        }

        [Fact]
        public void SearchBaseConverter_Movie()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new SearchBaseConverter());

            SearchMovie original = new SearchMovie();
            original.OriginalTitle = "Hello world";

            string json = JsonConvert.SerializeObject(original, settings);
            SearchMovie result = JsonConvert.DeserializeObject<SearchBase>(json, settings) as SearchMovie;

            Assert.NotNull(result);
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.OriginalTitle, result.OriginalTitle);
        }

        [Fact]
        public void SearchBaseConverter_Tv()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new SearchBaseConverter());

            SearchTv original = new SearchTv();
            original.OriginalName = "Hello world";

            string json = JsonConvert.SerializeObject(original, settings);
            SearchTv result = JsonConvert.DeserializeObject<SearchBase>(json, settings) as SearchTv;

            Assert.NotNull(result);
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.OriginalName, result.OriginalName);
        }

        [Fact]
        public void SearchBaseConverter_Person()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new SearchBaseConverter());

            SearchPerson original = new SearchPerson();
            original.Name = "Hello world";

            string json = JsonConvert.SerializeObject(original, settings);
            SearchPerson result = JsonConvert.DeserializeObject<SearchBase>(json, settings) as SearchPerson;

            Assert.NotNull(result);
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.Name, result.Name);
        }

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