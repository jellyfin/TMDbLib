using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Authentication;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientAuthenticationTests
    {
        private TestConfig _config;

        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        [TestMethod]
        public void TestAuthenticationRequestNewToken()
        {
            Token token = _config.Client.AuthenticationRequestAutenticationToken();

            Assert.IsNotNull(token);
            Assert.IsTrue(token.Success);
            Assert.IsNotNull(token.AuthenticationCallback);
            Assert.IsNotNull(token.ExpiresAt);
            Assert.IsNotNull(token.RequestToken);
        }

         //<remarks>
         //This requires manual intervention, as such it can not be included with the regular test set
         //To be able to execute this test request a token using the AuthenticationRequestAutenticationToken method.
         //Subsequently naviate to the AuthenticationCallback url specified on the returned object.
         //Log-in to a TMDb account and grant access when requested.
         //Use the RequestToken string previously provided as value for this test
         //</remarks>
        //[TestMethod]
        //public void TestAuthenticationUserAuthenticatedSessionSuccess()
        //{
        //    const string requestToken = "cb49e29af0473e78a4a489c91c6a8259407a343b";
        //    var session = _config.Client.AuthenticationGetUserSession(requestToken);

        //    Assert.IsNotNull(session);
        //    Assert.IsTrue(session.Success);
        //    Assert.IsNotNull(session.SessionId);
        //}

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void TestAuthenticationUserAuthenticatedSessionInvalidToken()
        {
            const string requestToken = "bla";
            _config.Client.AuthenticationGetUserSession(requestToken);

            // Should always throw exception
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void TestAuthenticationUserAuthenticatedSessionOldToken()
        {
            const string requestToken = "5f3a62c0d7977319e3d14adf1a2064c0c0938bcf";
            _config.Client.AuthenticationGetUserSession(requestToken);

            // Should always throw exception
            Assert.Fail();
        }

        [TestMethod]
        public void TestAuthenticationCreateGuestSession()
        {
            GuestSession guestSession = _config.Client.AuthenticationCreateGuestSession();

            Assert.IsNotNull(guestSession);
            Assert.IsTrue(guestSession.Success);
            Assert.IsNotNull(guestSession.ExpiresAt);
            Assert.IsNotNull(guestSession.GuestSessionId);
        }
    }
}
