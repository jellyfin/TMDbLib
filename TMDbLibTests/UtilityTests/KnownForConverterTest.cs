using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class KnownForConverterTest : TestBase
    {
        [Fact]
        public async Task KnownForConverter_Movie()
        {
            KnownForMovie original = new KnownForMovie();
            original.OriginalTitle = "Hello world";

            string json = Serializer.SerializeToString(original);
            KnownForMovie result = Serializer.DeserializeFromString<KnownForBase>(json) as KnownForMovie;
            
            Assert.Equal(original.Title, result.Title);
            await Verify(new
            {
                json,
                result
            });
        }

        [Fact]
        public async Task KnownForConverter_Tv()
        {
            KnownForTv original = new KnownForTv();
            original.OriginalName = "Hello world";
            
            string json = Serializer.SerializeToString(original);
            KnownForTv result = Serializer.DeserializeFromString<KnownForBase>(json) as KnownForTv;
            
            Assert.Equal(original.OriginalName, result.OriginalName);
            await Verify(new
            {
                json,
                result
            });
        }

        /// <summary>
        /// Tests the KnownForConverter
        /// </summary>
        [Fact]
        public async Task TestJsonKnownForConverter()
        {
            SearchContainer<SearchPerson> result = await TMDbClient.SearchPersonAsync("Willis");

            Assert.NotNull(result?.Results);

            List<KnownForBase> knownForList = result.Results.SelectMany(s => s.KnownFor).ToList();
            Assert.True(knownForList.Any());

            Assert.Contains(knownForList, item => item.MediaType == MediaType.Tv && item is KnownForTv);
            Assert.Contains(knownForList, item => item.MediaType == MediaType.Movie && item is KnownForMovie);
        }
    }
}