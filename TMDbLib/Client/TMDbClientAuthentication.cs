using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Creates a new guest session for rating media without a TMDb account.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The guest session.</returns>
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
    /// Creates a user session from a validated request token.
    /// </summary>
    /// <param name="initialRequestToken">A request token previously validated with user credentials.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user session.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the request token is invalid or unvalidated.</exception>
    public async Task<UserSession?> AuthenticationGetUserSessionAsync(string initialRequestToken, CancellationToken cancellationToken = default)
    {
        var request = _client.Create("authentication/session/new");
        request.AddParameter("request_token", initialRequestToken);

        try
        {
            using RestResponse<UserSession> response = await request.Get<UserSession>(cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException();
            }

            return await response.GetDataObject().ConfigureAwait(false);
        }
        catch (TMDbAuthenticationException ex)
        {
            throw new UnauthorizedAccessException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Convenience method that requests, validates, and exchanges a token for a user session.
    /// </summary>
    /// <param name="username">A valid TMDb username.</param>
    /// <param name="password">The password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user session.</returns>
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
    /// Requests a new authentication token.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A request token that must be validated before creating a session.</returns>
    public async Task<Token?> AuthenticationRequestAutenticationTokenAsync(CancellationToken cancellationToken = default)
    {
        var request = _client.Create("authentication/token/new");

        using var response = await request.Get<Token>(cancellationToken).ConfigureAwait(false);
        var token = await response.GetDataObject().ConfigureAwait(false);

        token?.AuthenticationCallback = response.GetHeader("Authentication-Callback");

        return token;
    }

    /// <summary>
    /// Validates a request token with user credentials.
    /// </summary>
    /// <param name="initialRequestToken">The request token to validate.</param>
    /// <param name="username">The TMDb username.</param>
    /// <param name="password">The TMDb password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the credentials are invalid.</exception>
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
        catch (TMDbAuthenticationException ex)
        {
            throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided user credentials are invalid.", ex);
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

    /// <summary>
    /// Validates the configured API key.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the API key is valid.</returns>
    public async Task<bool> AuthenticationValidateApiKeyAsync(CancellationToken cancellationToken = default)
    {
        var request = _client.Create("authentication");

        using var response = await request.Get(cancellationToken).ConfigureAwait(false);

        return response.IsValid;
    }

    /// <summary>
    /// Deletes a user session (logs the user out).
    /// </summary>
    /// <param name="sessionId">The session id to invalidate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the session was deleted.</returns>
    public async Task<bool> AuthenticationDeleteSessionAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var request = _client.Create("authentication/session");
        request.SetBody(new { session_id = sessionId });

        using var response = await request.Delete(cancellationToken).ConfigureAwait(false);

        return response.IsValid;
    }

    /// <summary>
    /// Converts a v4 access token into a v3 session.
    /// </summary>
    /// <param name="accessToken">A valid v4 access token.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user session.</returns>
    public async Task<UserSession?> AuthenticationCreateSessionFromV4Async(string accessToken, CancellationToken cancellationToken = default)
    {
        var request = _client.Create("authentication/session/convert/4");
        request.SetBody(new { access_token = accessToken });

        try
        {
            using var response = await request.Post<UserSession>(cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException();
            }

            return await response.GetDataObject().ConfigureAwait(false);
        }
        catch (TMDbAuthenticationException ex)
        {
            throw new UnauthorizedAccessException(ex.Message, ex);
        }
    }
}
