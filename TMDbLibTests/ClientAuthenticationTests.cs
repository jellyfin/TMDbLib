using System;
using System.Threading.Tasks;
using TMDbLibTests.Exceptions;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb authentication functionality.
/// </summary>
public class ClientAuthenticationTests : TestBase
{
    public ClientAuthenticationTests()
    {
        if (string.IsNullOrWhiteSpace(TestConfig.Username) || string.IsNullOrWhiteSpace(TestConfig.Password))
        {
            throw new ConfigurationErrorsException("You need to provide a username and password or some tests won't be able to execute.");
        }
    }

    /// <summary>
    /// Tests that requesting a new authentication token returns a valid token.
    /// </summary>
    [Fact]
    public async Task TestAuthenticationRequestNewToken()
    {
        Token token = await TMDbClient.AuthenticationRequestAutenticationTokenAsync();

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

    /// <summary>
    /// Tests that attempting to create a user session with an invalid token throws UnauthorizedAccessException.
    /// </summary>
    [Fact]
    public async Task TestAuthenticationUserAuthenticatedSessionInvalidTokenAsync()
    {
        const string requestToken = "bla";

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => TMDbClient.AuthenticationGetUserSessionAsync(requestToken));
    }

    /// <summary>
    /// Tests that validating a user token with valid credentials succeeds.
    /// </summary>
    /// <remarks>
    /// Requires a valid test user to be assigned
    /// </remarks>
    [Fact]
    public async Task TestAuthenticationGetUserSessionApiUserValidationSuccessAsync()
    {
        Token token = await TMDbClient.AuthenticationRequestAutenticationTokenAsync();

        await TMDbClient.AuthenticationValidateUserTokenAsync(token.RequestToken, TestConfig.Username, TestConfig.Password);
    }

    /// <summary>
    /// Tests that validating a user token with invalid credentials throws UnauthorizedAccessException.
    /// </summary>
    [Fact]
    public async Task TestAuthenticationGetUserSessionApiUserValidationInvalidLoginAsync()
    {
        Token token = await TMDbClient.AuthenticationRequestAutenticationTokenAsync();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => TMDbClient.AuthenticationValidateUserTokenAsync(token.RequestToken, "bla", "bla"));
    }

    /// <summary>
    /// Tests that creating a user session with valid username and password succeeds and returns a valid session.
    /// </summary>
    /// <remarks>
    /// Requires a valid test user to be assigned
    /// </remarks>
    [Fact]
    public async Task AuthenticationGetUserSessionWithLoginSuccess()
    {
        UserSession session = await TMDbClient.AuthenticationGetUserSessionAsync(TestConfig.Username, TestConfig.Password);

        Assert.NotNull(session);
        Assert.True(session.Success);
        Assert.NotNull(session.SessionId);
    }

    /// <summary>
    /// Tests that attempting to create a user session with an expired token throws UnauthorizedAccessException.
    /// </summary>
    [Fact]
    public async Task TestAuthenticationUserAuthenticatedSessionOldTokenAsync()
    {
        const string requestToken = "5f3a62c0d7977319e3d14adf1a2064c0c0938bcf";

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => TMDbClient.AuthenticationGetUserSessionAsync(requestToken));
    }

    /// <summary>
    /// Tests that creating a guest session returns a valid guest session ID.
    /// </summary>
    [Fact]
    public async Task TestAuthenticationCreateGuestSessionAsync()
    {
        GuestSession guestSession = await TMDbClient.AuthenticationCreateGuestSessionAsync();

        Assert.NotNull(guestSession);
        Assert.True(guestSession.Success);
        Assert.NotNull(guestSession.GuestSessionId);
    }
}
