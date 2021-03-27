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

namespace TMDbLibTests
{
    public class ClientCompanyTests : TestBase
    {
        private static readonly Dictionary<CompanyMethods, Func<Company, object>> Methods;

        static ClientCompanyTests()
        {
            Methods = new Dictionary<CompanyMethods, Func<Company, object>>
            {
                [CompanyMethods.Movies] = company => company.Movies
            };
        }

        [Fact]
        public async Task TestCompaniesExtrasNoneAsync()
        {
            Company company = await TMDbClient.GetCompanyAsync(IdHelper.TwentiethCenturyFox);

            // Test all extras, ensure none of them exist
            foreach (Func<Company, object> selector in Methods.Values)
            {
                Assert.Null(selector(company));
            }
        }

        [Fact]
        public async Task TestCompaniesExtrasExclusive()
        {
            await TestMethodsHelper.TestGetExclusive(Methods, extras => TMDbClient.GetCompanyAsync(IdHelper.TwentiethCenturyFox, extras));
        }

        [Fact]
        public async Task TestCompaniesExtrasAllAsync()
        {
            await TestMethodsHelper.TestGetAll(Methods, combined => TMDbClient.GetCompanyAsync(IdHelper.TwentiethCenturyFox, combined), async company =>
            {
                // Reduce testdata
                company.Movies.Results = company.Movies.Results.OrderBy(s => s.Id).Take(1).ToList();

                await Verify(company, settings => settings.IgnoreProperty(nameof(company.Movies.TotalPages), nameof(company.Movies.TotalResults)));
            });
        }

        [Fact]
        public async Task TestCompanyMissingAsync()
        {
            Company company = await TMDbClient.GetCompanyAsync(IdHelper.MissingID);

            Assert.Null(company);
        }

        [Fact]
        public async Task TestCompaniesMoviesAsync()
        {
            //GetCompanyMoviesAsync(int id, string language, int page = -1)
            SearchContainerWithId<SearchMovie> resp = await TMDbClient.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox);
            SearchContainerWithId<SearchMovie> respPage2 = await TMDbClient.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox, 2);
            SearchContainerWithId<SearchMovie> respItalian = await TMDbClient.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox, "it");

            Assert.NotEmpty(resp.Results);
            Assert.NotEmpty(respPage2.Results);
            Assert.NotEmpty(respItalian.Results);

            bool allTitlesIdentical = true;
            for (int index = 0; index < resp.Results.Count; index++)
            {
                Assert.Equal(resp.Results[index].Id, respItalian.Results[index].Id);

                if (resp.Results[index].Title != respItalian.Results[index].Title)
                    allTitlesIdentical = false;
            }

            Assert.False(allTitlesIdentical);
        }

        [Fact]
        public async Task TestCompaniesImagesAsync()
        {
            // Get config
            await TMDbClient.GetConfigAsync();

            // Test image url generator
            Company company = await TMDbClient.GetCompanyAsync(IdHelper.TwentiethCenturyFox);

            Uri url = TMDbClient.GetImageUrl("original", company.LogoPath);
            Uri urlSecure = TMDbClient.GetImageUrl("original", company.LogoPath, true);

            await Verify(new
            {
                url,
                urlSecure
            });
        }
    }
}