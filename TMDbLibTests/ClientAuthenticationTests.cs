using System;
using TMDbLibTests.Exceptions;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLibTests.Helpers;
using TMDbLibTests.TestFramework;

namespace TMDbLibTests
{
    /// <summary>
    /// https://www.themoviedb.org/documentation/api/sessions
    /// </summary>
    public class ClientAuthenticationTests : TestBase
    {
        public ClientAuthenticationTests(TestConfig testConfig) : base(testConfig)
        {
            if (string.IsNullOrWhiteSpace(Config.Username) || string.IsNullOrWhiteSpace(Config.Password))
                throw new ConfigurationErrorsException("You need to provide a username and password or some tests won't be able to execute.");
        }

        [Fact]
        public void TestAuthenticationRequestNewToken()
        {
            Token token = Config.Client.AuthenticationRequestAutenticationTokenAsync().Sync();

            Assert.NotNull(token);
            Assert.True(token.Success);
            Assert.NotNull(token.AuthenticationCallback);
            Assert.NotNull(token.ExpiresAt);
            Assert.NotNull(token.RequestToken);
        }

        //<remarks>
        //This requires manual intervention, as such it can not be included with the regular test set
        //To be able to execute this test request a token using the AuthenticationRequestAutenticationTokenAsync method.
        //Subsequently naviate to the AuthenticationCallback url specified on the returned object.
        //Log-in to a TMDb account and grant access when requested.
        //Use the RequestToken string previously provided as value for this test
        //</remarks>
        //[Fact]
        //public void TestAuthenticationUserAuthenticatedSessionSuccess()
        //{
        //    const string requestToken = "cb49e29af0473e78a4a489c91c6a8259407a343b";
        //    UserSession session = _config.Client.AuthenticationGetUserSessionAsync(requestToken);

        //    Assert.NotNull(session);
        //    Assert.True(session.Success);
        //    Assert.NotNull(session.SessionId);
        //}

        [Fact]
        public void TestAuthenticationUserAuthenticatedSessionInvalidToken()
        {
            const string requestToken = "bla";

            Assert.Throws<UnauthorizedAccessException>(() => Config.Client.AuthenticationGetUserSessionAsync(requestToken).Sync());
        }

        /// <remarks>
        /// Requires a valid test user to be assigned
        /// </remarks>
        [Fact]
        public void TestAuthenticationGetUserSessionApiUserValidationSuccess()
        {
            Token token = Config.Client.AuthenticationRequestAutenticationTokenAsync().Sync();

            Config.Client.AuthenticationValidateUserTokenAsync(token.RequestToken, Config.Username, Config.Password).Sync();
        }

        [Fact]
        public void TestAuthenticationGetUserSessionApiUserValidationInvalidLogin()
        {
            Token token = Config.Client.AuthenticationRequestAutenticationTokenAsync().Sync();

            Assert.Throws<UnauthorizedAccessException>(() => Config.Client.AuthenticationValidateUserTokenAsync(token.RequestToken, "bla", "bla").Sync());
        }

        /// <remarks>
        /// Requires a valid test user to be assigned
        /// </remarks>
        [Fact]
        public void AuthenticationGetUserSessionWithLoginSuccess()
        {
            UserSession session = Config.Client.AuthenticationGetUserSessionAsync(Config.Username, Config.Password).Result;

            Assert.NotNull(session);
            Assert.True(session.Success);
            Assert.NotNull(session.SessionId);
        }

        [Fact]
        public void TestAuthenticationUserAuthenticatedSessionOldToken()
        {
            const string requestToken = "5f3a62c0d7977319e3d14adf1a2064c0c0938bcf";

            Assert.Throws<UnauthorizedAccessException>(() => Config.Client.AuthenticationGetUserSessionAsync(requestToken).Sync());
        }

        [Fact]
        public void TestAuthenticationCreateGuestSession()
        {
            GuestSession guestSession = Config.Client.AuthenticationCreateGuestSessionAsync().Sync();

            Assert.NotNull(guestSession);
            Assert.True(guestSession.Success);
            Assert.NotNull(guestSession.ExpiresAt);
            Assert.NotNull(guestSession.GuestSessionId);
        }
    }
}
