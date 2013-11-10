using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientAccountTests
    {
        private TestConfig _config;
        private const string GuestTestSessionId = "0c81565c80905bbfd685782a907ee73d";

        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();

            if (string.IsNullOrWhiteSpace(_config.UserSessionId))
                throw new ConfigurationErrorsException("To successfully complete the ClientAccountTests you will need to specify a valid 'UserSessionId' in the test config file");

        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void TestAccountGetDetailsGuestAccount()
        {
            var account = _config.Client.AccountGetDetails(GuestTestSessionId);
        }

        [TestMethod]
        public void TestAccountGetDetailsUserAccount()
        {
            var account = _config.Client.AccountGetDetails(_config.UserSessionId);

            // Naturally the specified account must have these values populated for the test to pass
            Assert.IsNotNull(account);
            Assert.IsTrue(account.Id> 1);
            Assert.IsNotNull(account.Name);
            Assert.IsNotNull(account.Username);
            Assert.IsNotNull(account.Iso_3166_1);
            Assert.IsNotNull(account.Iso_639_1);
        }

        //[TestMethod]
        //public void TestAccountGetListsGuestAccount()
        //{
        //    TestHelpers.SearchPages(i => _config.Client.AccountGetLists(GuestTestSessionId));

        //    var lists = _config.Client.AccountGetLists(GuestTestSessionId);

        //    // Naturally the specified account must have these values populated for the test to pass
        //    var list = lists.Results[0];
        //    Assert.IsNotNull(list.Id);
        //    Assert.IsNotNull(list.CreatedBy);
        //}
    }
}
