using System.Threading.Tasks;
using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Converters;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class SearchBaseConverterTest : TestBase
    {
        [Fact]
        public async Task SearchBaseConverter_Movie()
        {
            SearchMovie original = new SearchMovie();
            original.OriginalTitle = "Hello world";
            
            string json = Serializer.SerializeToString(original);
            SearchMovie result = Serializer.DeserializeFromString<SearchBase>(json) as SearchMovie;
            
            Assert.Equal(original.OriginalTitle, result.OriginalTitle);
            await Verify(new
            {
                json,
                result
            });
        }

        [Fact]
        public async Task SearchBaseConverter_Tv()
        {
            SearchTv original = new SearchTv();
            original.OriginalName = "Hello world";
            
            string json = Serializer.SerializeToString(original);
            SearchTv result = Serializer.DeserializeFromString<SearchBase>(json) as SearchTv;
            
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.OriginalName, result.OriginalName);
            await Verify(new
            {
                json,
                result
            });
        }

        [Fact]
        public async Task SearchBaseConverter_Person()
        {
            SearchPerson original = new SearchPerson();
            original.Name = "Hello world";
            
            string json = Serializer.SerializeToString(original);
            SearchPerson result = Serializer.DeserializeFromString<SearchBase>(json) as SearchPerson;
            
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.Name, result.Name);
            await Verify(new
            {
                json,
                result
            });
        }

        /// <summary>
        /// Tests the SearchBaseConverter
        /// </summary>
        [Fact]
        public async Task TestSearchBaseConverter()
        {
            await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchMultiAsync("Jobs", i));
            SearchContainer<SearchBase> result = await TMDbClient.SearchMultiAsync("Jobs");

            Assert.NotNull(result);
            Assert.NotNull(result.Results);

            Assert.Contains(result.Results, item => item.MediaType == MediaType.Tv && item is SearchTv);
            Assert.Contains(result.Results, item => item.MediaType == MediaType.Movie && item is SearchMovie);
            Assert.Contains(result.Results, item => item.MediaType == MediaType.Person && item is SearchPerson);
        }
    }
}