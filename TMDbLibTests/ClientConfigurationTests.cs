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

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb configuration functionality.
/// </summary>
public class ClientConfigurationTests : TestBase
{
    /// <summary>
    /// Tests that retrieving the API configuration returns valid configuration data.
    /// </summary>
    [Fact]
    public async Task TestConfigurationAsync()
    {
        var result = await TMDbClient.GetAPIConfiguration();
        Assert.NotNull(result);

        await Verify(result);
    }

    /// <summary>
    /// Tests that retrieving primary translations returns a valid list including expected translations.
    /// </summary>
    [Fact]
    public async Task TestPrimaryTranslationsAsync()
    {
        var result = await TMDbClient.GetPrimaryTranslationsAsync();
        Assert.NotNull(result);

        Assert.Contains("da-DK", result);
    }

    /// <summary>
    /// Tests that retrieving the country list returns valid country data.
    /// </summary>
    [Fact]
    public async Task TestCountryListAsync()
    {
        var result = await TMDbClient.GetCountriesAsync();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        var single = result.Single(s => s.EnglishName == "Denmark");

        await Verify(single);
    }

    /// <summary>
    /// Tests that retrieving the language list returns valid language data.
    /// </summary>
    [Fact]
    public async Task TestLanguageListAsync()
    {
        var result = await TMDbClient.GetLanguagesAsync();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        var single = result.Single(s => s.Name == "Dansk");

        await Verify(single);
    }

    /// <summary>
    /// Tests that retrieving the timezones list returns valid timezone data.
    /// </summary>
    [Fact]
    public async Task TestTimezonesListAsync()
    {
        var result = await TMDbClient.GetTimezonesAsync();
        Assert.NotNull(result);
        Assert.NotNull(result.List);
        Assert.NotEmpty(result.List);
        var single = result.List["DK"];

        await Verify(single);
    }

    /// <summary>
    /// Tests that retrieving the job list returns valid job department data.
    /// </summary>
    [Fact]
    public async Task TestJobListAsync()
    {
        var jobs = await TMDbClient.GetJobsAsync();
        Assert.NotNull(jobs);
        Assert.NotEmpty(jobs);
        var single = jobs.Single(s => s.Department == "Writing");

        await Verify(single);
    }
}
