using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Creates a new guest session for rating movies and TV shows without a registered TMDb account.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A guest session object containing the guest session ID.</returns>
    public async Task<GuestSession?> AuthenticationCreateGuestSessionAsync(CancellationToken cancellationToken = default)
    {
        var request = _client.Create("authentication/guest_session/new");
        // {
        //    DateFormat = "yyyy-MM-dd HH:mm:ss UTC"
        // };

        var response = await request.GetOfT<GuestSession>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Creates a user session using a validated request token.
    /// </summary>
    /// <param name="initialRequestToken">A request token that has been validated with user credentials.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A user session object containing the session ID for authenticated API requests.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the request token is invalid or has not been validated.</exception>
    public async Task<UserSession?> AuthenticationGetUserSessionAsync(string initialRequestToken, CancellationToken cancellationToken = default)
    {
        var request = _client.Create("authentication/session/new");
        request.AddParameter("request_token", initialRequestToken);

        using RestResponse<UserSession> response = await request.Get<UserSession>(cancellationToken).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        return await response.GetDataObject().ConfigureAwait(false);
    }

    /// <summary>
    /// Conveniance method combining 'AuthenticationRequestAutenticationTokenAsync', 'AuthenticationValidateUserTokenAsync' and 'AuthenticationGetUserSessionAsync'.
    /// </summary>
    /// <param name="username">A valid TMDb username.</param>
    /// <param name="password">The passoword for the provided login.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A user session object containing the session ID for authenticated API requests.</returns>
    public async Task<UserSession?> AuthenticationGetUserSessionAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var token = await AuthenticationRequestAutenticationTokenAsync(cancellationToken).ConfigureAwait(false);
        if (token?.RequestToken is null)
        {
            return null;
        }

        await AuthenticationValidateUserTokenAsync(token.RequestToken, username, password, cancellationToken).ConfigureAwait(false);

        return await AuthenticationGetUserSessionAsync(token.RequestToken, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Requests a new authentication token from TMDb for user authentication.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A token object containing the request token that must be validated before creating a session.</returns>
    public async Task<Token?> AuthenticationRequestAutenticationTokenAsync(CancellationToken cancellationToken = default)
    {
        var request = _client.Create("authentication/token/new");

        using var response = await request.Get<Token>(cancellationToken).ConfigureAwait(false);
        var token = await response.GetDataObject().ConfigureAwait(false);

        token?.AuthenticationCallback = response.GetHeader("Authentication-Callback");

        return token;
    }

    /// <summary>
    /// Validates a request token with TMDb user credentials.
    /// </summary>
    /// <param name="initialRequestToken">The request token to validate.</param>
    /// <param name="username">The TMDb username.</param>
    /// <param name="password">The TMDb password.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the provided user credentials are invalid.</exception>
    public async Task AuthenticationValidateUserTokenAsync(string initialRequestToken, string username, string password, CancellationToken cancellationToken = default)
    {
        var request = _client.Create("authentication/token/validate_with_login");
        request.AddParameter("request_token", initialRequestToken);
        request.AddParameter("username", username);
        request.AddParameter("password", password);

        RestResponse response;
        try
        {
            response = await request.Get(cancellationToken).ConfigureAwait(false);
        }
        catch (AggregateException ex)
        {
            var inner = ex.InnerException;
            if (inner is not null)
            {
                throw inner;
            }

            throw;
        }

        using (response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided user credentials are invalid.");
            }
        }
    }
}
