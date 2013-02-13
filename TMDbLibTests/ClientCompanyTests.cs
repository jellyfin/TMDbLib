using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
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

        // This is relevant when (and if) Companies get more methods
        //[TestMethod]
        //public void TestCompaniesExtrasExclusive()
        //{
        //    // Test combinations of extra methods, fetch everything but each one, ensure all but the one exist
        //    foreach (CompanyMethods method in _methods.Keys)
        //    {
        //        // Prepare the combination exlcuding the one (method).
        //        CompanyMethods combo = _methods.Keys.Except(new[] { method }).Aggregate((movieMethod, accumulator) => movieMethod | accumulator);

        //        // Fetch data
        //        Company company = _config.Client.GetCompany(TwentiethCenturyFox, combo);

        //        // Ensure we have all pieces
        //        foreach (CompanyMethods expectedMethod in _methods.Keys.Except(new[] { method }))
        //            Assert.IsNotNull(_methods[expectedMethod](company));

        //        // .. except the method we're testing.
        //        Assert.IsNull(_methods[method](company));
        //    }
        //}

        [TestMethod]
        public void TestCompaniesGetters()
        {
            //GetCompanyMovies(int id, string language, int page = -1)
            {
                var resp = _config.Client.GetCompanyMovies(TwentiethCenturyFox);
                var respPage2 = _config.Client.GetCompanyMovies(TwentiethCenturyFox,2);
                var respItalian = _config.Client.GetCompanyMovies(TwentiethCenturyFox, "it");

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
