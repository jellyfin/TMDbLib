using System.Collections.Generic;
using Xunit;
using TMDbLib.Objects.Timezones;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using TMDbLib.Objects.Countries;
using TMDbLib.Objects.Languages;

namespace TMDbLibTests
{
    public class ClientConfigurationTests : TestBase
    {
        [Fact]
        public void TestCountryList()
        {
            List<Country> result = Config.Client.GetCountriesAsync().Sync();

            Assert.NotNull(result);
            Assert.True(result.Count > 200);

            Assert.Contains(result, c => c.EnglishName == "Denmark" && c.Iso_3166_1 == "DK");
        }

        [Fact]
        public void TestLanguageList()
        {
            List<Language> result = Config.Client.GetLanguagesAsync().Sync();

            Assert.NotNull(result);
            Assert.True(result.Count > 180);

            Assert.Contains(result, l => l.Name == "Dansk" && l.EnglishName == "Danish" && l.Iso_639_1 == "da");
        }

        [Fact]
        public void TestTimezonesList()
        {
            Timezones result = Config.Client.GetTimezonesAsync().Sync();

            Assert.NotNull(result);
            Assert.True(result.List.Count > 200);

            List<string> item = result.List["DK"];
            Assert.NotNull(item);
            Assert.Equal(1, item.Count);
            Assert.Equal("Europe/Copenhagen", item[0]);
        }
    }
}