using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Companies;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

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
            [CompanyMethods.AlternativeNames] = company => company.AlternativeNames,
            [CompanyMethods.Images] = company => company.Images
        };
    }

    /// <summary>
    /// Tests that retrieving a company without extras does not include extra data.
    /// </summary>
    [Fact]
    public async Task TestCompaniesExtrasNoneAsync()
    {
        var company = await TMDbClient.GetCompanyAsync(IdHelper.TwentiethCenturyFox, cancellationToken: TestContext.Current.CancellationToken);
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
        }, async company => await Verify(company));
    }

    /// <summary>
    /// Tests that attempting to retrieve a non-existent company returns null.
    /// </summary>
    [Fact]
    public async Task TestCompanyMissingAsync()
    {
        var company = await TMDbClient.GetCompanyAsync(IdHelper.MissingID, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Null(company);
    }

    /// <summary>
    /// Tests that the standalone GetCompanyAlternativeNamesAsync endpoint returns a result
    /// with the company's id and a (possibly empty) list of alternative names.
    /// </summary>
    [Fact]
    public async Task TestCompanyAlternativeNamesAsync()
    {
        var result = await TMDbClient.GetCompanyAlternativeNamesAsync(IdHelper.TwentiethCenturyFox, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal(IdHelper.TwentiethCenturyFox, result.Id);
        Assert.NotNull(result.Results);
    }

    /// <summary>
    /// Tests that the standalone GetCompanyImagesAsync endpoint returns logos for the company.
    /// </summary>
    [Fact]
    public async Task TestCompanyImagesEndpointAsync()
    {
        var result = await TMDbClient.GetCompanyImagesAsync(IdHelper.TwentiethCenturyFox, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal(IdHelper.TwentiethCenturyFox, result.Id);
        Assert.NotNull(result.Logos);
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
        var company = await TMDbClient.GetCompanyAsync(IdHelper.TwentiethCenturyFox, cancellationToken: TestContext.Current.CancellationToken);
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
