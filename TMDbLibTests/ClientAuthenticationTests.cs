using System;
using System.Threading.Tasks;
using TMDbLibTests.Exceptions;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    /// <summary>
    /// https://www.themoviedb.org/documentation/api/sessions
    /// </summary>
    public class ClientAuthenticationTests : TestBase
    {
        public ClientAuthenticationTests()
        {
            if (string.IsNullOrWhiteSpace(Config.Username) || string.IsNullOrWhiteSpace(Config.Password))
                throw new ConfigurationErrorsException("You need to provide a username and password or some tests won't be able to execute.");
        }

        [Fact]
        public async Task TestAuthenticationRequestNewToken()
        {
            Token token = await Config.Client.AuthenticationRequestAutenticationTokenAsync();

            Assert.NotNull(token);
            Assert.True(token.Success);
            Assert.NotNull(token.AuthenticationCallback);
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
        public async Task TestAuthenticationUserAuthenticatedSessionInvalidTokenAsync()
        {
            const string requestToken = "bla";

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => Config.Client.AuthenticationGetUserSessionAsync(requestToken));
        }

        /// <remarks>
        /// Requires a valid test user to be assigned
        /// </remarks>
        [Fact]
        public async Task TestAuthenticationGetUserSessionApiUserValidationSuccessAsync()
        {
            Token token = await Config.Client.AuthenticationRequestAutenticationTokenAsync();

            await Config.Client.AuthenticationValidateUserTokenAsync(token.RequestToken, Config.Username, Config.Password);
        }

        [Fact]
        public async Task TestAuthenticationGetUserSessionApiUserValidationInvalidLoginAsync()
        {
            Token token = await Config.Client.AuthenticationRequestAutenticationTokenAsync();

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => Config.Client.AuthenticationValidateUserTokenAsync(token.RequestToken, "bla", "bla"));
        }

        /// <remarks>
        /// Requires a valid test user to be assigned
        /// </remarks>
        [Fact]
        public async Task AuthenticationGetUserSessionWithLoginSuccess()
        {
            UserSession session = await Config.Client.AuthenticationGetUserSessionAsync(Config.Username, Config.Password);

            Assert.NotNull(session);
            Assert.True(session.Success);
            Assert.NotNull(session.SessionId);
        }

        [Fact]
        public async Task TestAuthenticationUserAuthenticatedSessionOldTokenAsync()
        {
            const string requestToken = "5f3a62c0d7977319e3d14adf1a2064c0c0938bcf";

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => Config.Client.AuthenticationGetUserSessionAsync(requestToken));
        }

        [Fact]
        public async Task TestAuthenticationCreateGuestSessionAsync()
        {
            GuestSession guestSession = await Config.Client.AuthenticationCreateGuestSessionAsync();

            Assert.NotNull(guestSession);
            Assert.True(guestSession.Success);
            Assert.NotNull(guestSession.GuestSessionId);
        }
    }
}
