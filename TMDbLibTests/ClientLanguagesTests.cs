using System.Collections.Generic;
using TMDbLib.Objects.Languages;
using Xunit;
using TMDbLib.Objects.Timezones;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientLanguagesTests : TestBase
    {
        [Fact]
        public void TestLanguageList()
        {
            List<Language> result = Config.Client.GetLanguagesAsync().Sync();

            Assert.NotNull(result);
            Assert.True(result.Count > 180);

            Assert.Contains(result, l => l.Name == "Dansk" && l.EnglishName == "Danish" && l.Iso_639_1 == "da");
        }
    }
}