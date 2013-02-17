using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientCompanyTests
    {
        private const int TwentiethCenturyFox = 25;

        private Dictionary<CompanyMethods, Func<Company, object>> _methods;
        private TestConfig _config;

        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();

            _methods = new Dictionary<CompanyMethods, Func<Company, object>>();
            _methods[CompanyMethods.Movies] = company => company.Movies;
        }

        [TestMethod]
        public void TestCompaniesExtrasNone()
        {
            Company company = _config.Client.GetCompany(TwentiethCenturyFox);

            Assert.IsNotNull(company);

            // TODO: Test all properties
            Assert.AreEqual("20th Century Fox", company.Name);

            // Test all extras, ensure none of them exist
            foreach (Func<Company, object> selector in _methods.Values)
            {
                Assert.IsNull(selector(company));
            }
        }

        [TestMethod]
        public void TestCompaniesExtrasExclusive()
        {
            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => _config.Client.GetCompany(id, extras), TwentiethCenturyFox);
        }

        [TestMethod]
        public void TestCompaniesExtrasAll()
        {
            CompanyMethods combinedEnum = _methods.Keys.Aggregate((methods, movieMethods) => methods | movieMethods);
            Company item = _config.Client.GetCompany(TwentiethCenturyFox, combinedEnum);

            TestMethodsHelper.TestGetAll(_methods, item);
        }

        [TestMethod]
        public void TestCompaniesGetters()
        {
            //GetCompanyMovies(int id, string language, int page = -1)
            {
                SearchContainerWithId<MovieResult> resp = _config.Client.GetCompanyMovies(TwentiethCenturyFox);
                SearchContainerWithId<MovieResult> respPage2 = _config.Client.GetCompanyMovies(TwentiethCenturyFox,2);
                SearchContainerWithId<MovieResult> respItalian = _config.Client.GetCompanyMovies(TwentiethCenturyFox, "it");

                Assert.IsNotNull(resp);
                Assert.IsNotNull(respPage2);
                Assert.IsNotNull(respItalian);

                Assert.IsTrue(resp.Results.Count > 0);
                Assert.IsTrue(respPage2.Results.Count > 0);
                Assert.IsTrue(respItalian.Results.Count > 0);

                bool allTitlesIdentical = true;
                for (int index = 0; index < resp.Results.Count; index++)
                {
                    Assert.AreEqual(resp.Results[index].Id, respItalian.Results[index].Id);

                    if (resp.Results[index].Title != respItalian.Results[index].Title)
                        allTitlesIdentical = false;
                }

                Assert.IsFalse(allTitlesIdentical);
            }
        }

        [TestMethod]
        public void TestCompaniesImages()
        {
            // Get config
            _config.Client.GetConfig();

            // Test image url generator
            Company company = _config.Client.GetCompany(TwentiethCenturyFox);

            Uri url = _config.Client.GetImageUrl("original", company.LogoPath);
            Uri urlSecure = _config.Client.GetImageUrl("original", company.LogoPath, true);

            Assert.IsTrue(TestHelpers.InternetUriExists(url));
            Assert.IsTrue(TestHelpers.InternetUriExists(urlSecure));
        }
    }
}
