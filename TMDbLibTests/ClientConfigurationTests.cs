using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Configuration;
using Xunit;
using TMDbLib.Objects.Timezones;
using TMDbLibTests.JsonHelpers;
using TMDbLib.Objects.Countries;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Languages;

namespace TMDbLibTests
{
    public class ClientConfigurationTests : TestBase
    {
        [Fact]
        public async Task TestConfigurationAsync()
        {
            APIConfiguration result = await TMDbClient.GetAPIConfiguration();

            await Verify(result);
        }

        [Fact]
        public async Task TestPrimaryTranslationsAsync()
        {
            List<string> result = await TMDbClient.GetPrimaryTranslationsAsync();

            Assert.Contains("da-DK", result);
        }

        [Fact]
        public async Task TestCountryListAsync()
        {
            List<Country> result = await TMDbClient.GetCountriesAsync();

            Assert.NotEmpty(result);
            Country single = result.Single(s => s.EnglishName == "Denmark");

            await Verify(single);
        }

        [Fact]
        public async Task TestLanguageListAsync()
        {
            List<Language> result = await TMDbClient.GetLanguagesAsync();

            Assert.NotEmpty(result);
            Language single = result.Single(s => s.Name == "Dansk");

            await Verify(single);
        }

        [Fact]
        public async Task TestTimezonesListAsync()
        {
            Timezones result = await TMDbClient.GetTimezonesAsync();

            Assert.NotEmpty(result.List);
            List<string> single = result.List["DK"];

            await Verify(single);
        }

        [Fact]
        public async Task TestJobListAsync()
        {
            List<Job> jobs = await TMDbClient.GetJobsAsync();

            Assert.NotEmpty(jobs);
            Job single = jobs.Single(s => s.Department == "Writing");

            await Verify(single);
        }
    }
}