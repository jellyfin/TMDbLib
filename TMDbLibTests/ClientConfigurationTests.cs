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

            Assert.NotNull(result);

            Assert.Contains(result.Images.BackdropSizes, c => c == "original");
        }

        [Fact]
        public async Task TestPrimaryTranslationsAsync()
        {
            List<string> result = await TMDbClient.GetPrimaryTranslationsAsync();

            Assert.NotNull(result);

            Assert.Contains(result, c => c == "da-DK");
        }

        [Fact]
        public async Task TestCountryListAsync()
        {
            List<Country> result = await TMDbClient.GetCountriesAsync();

            Assert.NotNull(result);
            Assert.True(result.Count > 200);

            Assert.Contains(result, c => c.EnglishName == "Denmark" && c.Iso_3166_1 == "DK");
        }

        [Fact]
        public async Task TestLanguageListAsync()
        {
            List<Language> result = await TMDbClient.GetLanguagesAsync();

            Assert.NotNull(result);
            Assert.True(result.Count > 180);

            Assert.Contains(result, l => l.Name == "Dansk" && l.EnglishName == "Danish" && l.Iso_639_1 == "da");
        }

        [Fact]
        public async Task TestTimezonesListAsync()
        {
            Timezones result = await TMDbClient.GetTimezonesAsync();

            Assert.NotNull(result);
            Assert.True(result.List.Count > 200);

            List<string> item = result.List["DK"];
            Assert.NotNull(item);
            Assert.Single(item);
            Assert.Equal("Europe/Copenhagen", item[0]);
        }

        [Fact]
        public async Task TestJobListAsync()
        {
            List<Job> jobs = await TMDbClient.GetJobsAsync();

            Assert.NotNull(jobs);
            Assert.True(jobs.Count > 0);

            Assert.True(jobs.All(job => !string.IsNullOrEmpty(job.Department)));
            Assert.True(jobs.All(job => job.Jobs != null));
            Assert.True(jobs.All(job => job.Jobs.Count > 0));
        }
    }
}