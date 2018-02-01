using System.Collections.Generic;
using TMDbLib.Objects.Countries;
using Xunit;
using TMDbLib.Objects.Timezones;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientCountriesTests : TestBase
    {
        [Fact]
        public void TestCountryList()
        {
            List<Country> result = Config.Client.GetCountriesAsync().Sync();

            Assert.NotNull(result);
            Assert.True(result.Count > 200);

            Assert.Contains(result, c => c.EnglishName == "Denmark" && c.Iso_3166_1 == "DK");
        }
    }
}