using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb company functionality.
/// </summary>
public class ClientCompanyTests : TestBase
{
    private static readonly Dictionary<CompanyMethods, Func<Company, object?>> Methods;

    static ClientCompanyTests()
    {
        Methods = new Dictionary<CompanyMethods, Func<Company, object?>>
        {
            [CompanyMethods.Movies] = company => company.Movies
        };
    }

    /// <summary>
    /// Tests that retrieving a company without extras does not include extra data.
    /// </summary>
    [Fact]
    public async Task TestCompaniesExtrasNoneAsync()
    {
        var company = await TMDbClient.GetCompanyAsync(IdHelper.TwentiethCenturyFox);
        Assert.NotNull(company);

        // Test all extras, ensure none of them exist
        foreach (var selector in Methods.Values)
        {
            Assert.Null(selector(company));
        }
    }

    /// <summary>
    /// Tests that requesting specific company extras returns only those extras.
    /// </summary>
    [Fact]
    public async Task TestCompaniesExtrasExclusive()
    {
        await TestMethodsHelper.TestGetExclusive(Methods, async extras =>
        {
            var result = await TMDbClient.GetCompanyAsync(IdHelper.TwentiethCenturyFox, extras);
            Assert.NotNull(result);
            return result;
        });
    }

    /// <summary>
    /// Tests that requesting all company extras returns all available extra data.
    /// </summary>
    [Fact]
    public async Task TestCompaniesExtrasAllAsync()
    {
        await TestMethodsHelper.TestGetAll(Methods, async combined =>
        {
            var result = await TMDbClient.GetCompanyAsync(IdHelper.TwentiethCenturyFox, combined);
            Assert.NotNull(result);
            return result;
        }, async company =>
        {
            // Reduce testdata
            Assert.NotNull(company.Movies);
            Assert.NotNull(company.Movies.Results);
            company.Movies.Results = company.Movies.Results.OrderBy(s => s.Id).Take(1).ToList();

            await Verify(company, settings => settings.IgnoreProperty(nameof(company.Movies.TotalPages), nameof(company.Movies.TotalResults)));
        });
    }

    /// <summary>
    /// Tests that attempting to retrieve a non-existent company returns null.
    /// </summary>
    [Fact]
    public async Task TestCompanyMissingAsync()
    {
        var company = await TMDbClient.GetCompanyAsync(IdHelper.MissingID);

        Assert.Null(company);
    }

    /// <summary>
    /// Tests that retrieving company movies returns results and supports language filtering and pagination.
    /// </summary>
    [Fact]
    public async Task TestCompaniesMoviesAsync()
    {
        //GetCompanyMoviesAsync(int id, string language, int page = -1)
        var resp = await TMDbClient.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox);
        var respPage2 = await TMDbClient.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox, 2);
        var respItalian = await TMDbClient.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox, "it");

        Assert.NotNull(resp);
        Assert.NotNull(respPage2);
        Assert.NotNull(respItalian);
        Assert.NotNull(resp.Results);
        Assert.NotNull(respPage2.Results);
        Assert.NotNull(respItalian.Results);
        Assert.NotEmpty(resp.Results);
        Assert.NotEmpty(respPage2.Results);
        Assert.NotEmpty(respItalian.Results);

        // Verify pagination works
        Assert.True(resp.TotalResults > resp.Results.Count, "Should have multiple pages of results");

        // Verify Italian localization returns translated titles (at least some should differ)
        // Note: API ordering can vary between languages, so we check if any titles differ
        var englishTitles = resp.Results.Select(r => r.Title).ToHashSet();
        var italianTitles = respItalian.Results.Select(r => r.Title).ToHashSet();
        Assert.False(englishTitles.SetEquals(italianTitles), "Italian results should have different titles than English");
    }

    /// <summary>
    /// Tests that company logo URLs are generated correctly for both secure and non-secure URLs.
    /// </summary>
    [Fact]
    public async Task TestCompaniesImagesAsync()
    {
        // Get config
        await TMDbClient.GetConfigAsync();

        // Test image url generator
        var company = await TMDbClient.GetCompanyAsync(IdHelper.TwentiethCenturyFox);
        Assert.NotNull(company);
        Assert.NotNull(company.LogoPath);

        Uri url = TMDbClient.GetImageUrl("original", company.LogoPath);
        Uri urlSecure = TMDbClient.GetImageUrl("original", company.LogoPath, true);

        await Verify(new
        {
            url,
            urlSecure
        });
    }
}
