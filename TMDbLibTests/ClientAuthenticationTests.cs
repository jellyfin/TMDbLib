using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Authentication;

namespace TMDbLibTests
{
    /// <summary>
    /// https://www.themoviedb.org/documentation/api/sessions
    /// </summary>
    [TestClass]
    public class ClientAuthenticationTests
    {
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();

            if (string.IsNullOrWhiteSpace(_config.Username) || string.IsNullOrWhiteSpace(_config.Password))
                throw new ConfigurationErrorsException("You need to provide a username and password or some tests won't be able to execute.");
        }

        [TestMethod]
        public void TestAuthenticationRequestNewToken()
        {
            Token token = _config.Client.AuthenticationRequestAutenticationToken().Result;

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
        //    UserSession session = _config.Client.AuthenticationGetUserSession(requestToken);

        //    Assert.IsNotNull(session);
        //    Assert.IsTrue(session.Success);
        //    Assert.IsNotNull(session.SessionId);
        //}

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void TestAuthenticationUserAuthenticatedSessionInvalidToken()
        {
            const string requestToken = "bla";

            try
            {
                _config.Client.AuthenticationGetUserSession(requestToken).Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }

            // Should always throw exception
            Assert.Fail();
        }

        /// <remarks>
        /// Requires a valid test user to be assigned
        /// </remarks>
        [TestMethod]
        public void TestAuthenticationGetUserSessionApiUserValidationSuccess()
        {
            Token token = _config.Client.AuthenticationRequestAutenticationToken().Result;

            _config.Client.AuthenticationValidateUserToken(token.RequestToken, _config.Username, _config.Password).Wait();
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void TestAuthenticationGetUserSessionApiUserValidationInvalidLogin()
        {
            Token token = _config.Client.AuthenticationRequestAutenticationToken().Result;

            try
            {
                _config.Client.AuthenticationValidateUserToken(token.RequestToken, "bla", "bla").Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }

            // Should always throw exception
            Assert.Fail();
        }

        /// <remarks>
        /// Requires a valid test user to be assigned
        /// </remarks>
        [TestMethod]
        public void AuthenticationGetUserSessionWithLoginSuccess()
        {
            UserSession session = _config.Client.AuthenticationGetUserSession(_config.Username, _config.Password).Result;

            Assert.IsNotNull(session);
            Assert.IsTrue(session.Success);
            Assert.IsNotNull(session.SessionId);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void TestAuthenticationUserAuthenticatedSessionOldToken()
        {
            const string requestToken = "5f3a62c0d7977319e3d14adf1a2064c0c0938bcf";
            try
            {
                _config.Client.AuthenticationGetUserSession(requestToken).Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }

            // Should always throw exception
            Assert.Fail();
        }

        [TestMethod]
        public void TestAuthenticationCreateGuestSession()
        {
            GuestSession guestSession = _config.Client.AuthenticationCreateGuestSession().Result;

            Assert.IsNotNull(guestSession);
            Assert.IsTrue(guestSession.Success);
            Assert.IsNotNull(guestSession.ExpiresAt);
            Assert.IsNotNull(guestSession.GuestSessionId);
        }
    }
}
