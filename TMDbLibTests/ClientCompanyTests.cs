using System;
using System.Collections.Generic;
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
        private static Dictionary<CompanyMethods, Func<Company, object>> _methods;

        public ClientCompanyTests()
        {
            _methods = new Dictionary<CompanyMethods, Func<Company, object>>
            {
                [CompanyMethods.Movies] = company => company.Movies
            };
        }

        [Fact]
        public async Task TestCompaniesExtrasNoneAsync()
        {
            Company company = await Config.Client.GetCompanyAsync(IdHelper.TwentiethCenturyFox);

            Assert.NotNull(company);

            // TODO: Test all properties
            Assert.Equal("20th Century Fox", company.Name);

            // Test all extras, ensure none of them exist
            foreach (Func<Company, object> selector in _methods.Values)
            {
                Assert.Null(selector(company));
            }
        }

        [Fact]
        public async Task TestCompaniesExtrasExclusive()
        {
            await TestMethodsHelper.TestGetExclusive(_methods, extras => Config.Client.GetCompanyAsync( IdHelper.TwentiethCenturyFox, extras));
        }

        [Fact]
        public async Task TestCompaniesExtrasAllAsync()
        {
            await TestMethodsHelper.TestGetAll(_methods, combined => Config.Client.GetCompanyAsync(IdHelper.TwentiethCenturyFox, combined));
        }

        [Fact]
        public async Task TestCompanyMissingAsync()
        {
            Company company = await Config.Client.GetCompanyAsync(IdHelper.MissingID);

            Assert.Null(company);
        }

        [Fact]
        public async Task TestCompaniesGettersAsync()
        {
            //GetCompanyMoviesAsync(int id, string language, int page = -1)
            SearchContainerWithId<SearchMovie> resp = await Config.Client.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox);
            SearchContainerWithId<SearchMovie> respPage2 = await Config.Client.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox, 2);
            SearchContainerWithId<SearchMovie> respItalian = await Config.Client.GetCompanyMoviesAsync(IdHelper.TwentiethCenturyFox, "it");

            Assert.NotNull(resp);
            Assert.NotNull(respPage2);
            Assert.NotNull(respItalian);

            Assert.True(resp.Results.Count > 0);
            Assert.True(respPage2.Results.Count > 0);
            Assert.True(respItalian.Results.Count > 0);

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
            await Config.Client.GetConfigAsync();

            // Test image url generator
            Company company = await Config.Client.GetCompanyAsync(IdHelper.TwentiethCenturyFox);

            Uri url = Config.Client.GetImageUrl("original", company.LogoPath);
            Uri urlSecure = Config.Client.GetImageUrl("original", company.LogoPath, true);

            Assert.True(await TestHelpers.InternetUriExistsAsync(url));
            Assert.True(await TestHelpers.InternetUriExistsAsync(urlSecure));
        }

        [Fact]
        public async Task TestCompaniesFullAsync()
        {
            Company company = await Config.Client.GetCompanyAsync(IdHelper.ColumbiaPictures);

            Assert.NotNull(company);

            Assert.Equal(IdHelper.ColumbiaPictures, company.Id);
            Assert.Equal("Columbia Pictures Industries, Inc. (CPII) is an American film production and distribution company. Columbia Pictures now forms part of the Columbia TriStar Motion Picture Group, owned by Sony Pictures Entertainment, a subsidiary of the Japanese conglomerate Sony. It is one of the leading film companies in the world, a member of the so-called Big Six. It was one of the so-called Little Three among the eight major film studios of Hollywood's Golden Age.", company.Description);
            Assert.Equal("Culver City, California", company.Headquarters);
            Assert.Equal("http://www.sonypictures.com/", company.Homepage);
            Assert.Equal("US", company.OriginCountry);
            Assert.Equal("/mjUSfXXUhMiLAA1Zq1TfStNSoLR.png", company.LogoPath);
            Assert.Equal("Columbia Pictures", company.Name);

            Assert.NotNull(company.ParentCompany);
            Assert.Equal(5752, company.ParentCompany.Id);
            Assert.Equal("/sFg00KK0vVq3oqvkCxRQWApYB83.png", company.ParentCompany.LogoPath);
            Assert.Equal("Sony Pictures Entertainment", company.ParentCompany.Name);
        }
    }
}