using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Converters;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class KnownForConverterTest : TestBase
    {
        [Fact]
        public void KnownForConverter_Movie()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new KnownForConverter());

            KnownForMovie original = new KnownForMovie();
            original.OriginalTitle = "Hello world";

            string json = JsonConvert.SerializeObject(original, settings);
            KnownForMovie result = JsonConvert.DeserializeObject<KnownForBase>(json, settings) as KnownForMovie;

            Assert.NotNull(result);
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.Title, result.Title);
        }

        [Fact]
        public void KnownForConverter_Tv()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new KnownForConverter());

            KnownForTv original = new KnownForTv();
            original.OriginalName = "Hello world";

            string json = JsonConvert.SerializeObject(original, settings);
            KnownForTv result = JsonConvert.DeserializeObject<KnownForBase>(json, settings) as KnownForTv;

            Assert.NotNull(result);
            Assert.Equal(original.MediaType, result.MediaType);
            Assert.Equal(original.OriginalName, result.OriginalName);
        }

        /// <summary>
        /// Tests the KnownForConverter
        /// </summary>
        [Fact]
        public async Task TestJsonKnownForConverter()
        {
            SearchContainer<SearchPerson> result = await Config.Client.SearchPersonAsync("Willis");

            Assert.NotNull(result);
            Assert.NotNull(result.Results);

            List<KnownForBase> knownForList = result.Results.SelectMany(s => s.KnownFor).ToList();
            Assert.True(knownForList.Any());

            Assert.Contains(knownForList, item => item.MediaType == MediaType.Tv && item is KnownForTv);
            Assert.Contains(knownForList, item => item.MediaType == MediaType.Movie && item is KnownForMovie);
        }
    }
}