using System.Collections.Generic;
using System.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class KnownForConverterTest : TestBase
    {
        /// <summary>
        /// Tests the KnownForConverter
        /// </summary>
        [Fact]
        public void TestJsonKnownForConverter()
        {
            SearchContainer<SearchPerson> result = Config.Client.SearchPersonAsync("Willis").Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);

            List<KnownForBase> knownForList = result.Results.SelectMany(s => s.KnownFor).ToList();
            Assert.True(knownForList.Any());

            Assert.Contains(knownForList, item => item.MediaType == MediaType.Tv && item is KnownForTv);
            Assert.Contains(knownForList, item => item.MediaType == MediaType.Movie && item is KnownForMovie);
        }
    }
}