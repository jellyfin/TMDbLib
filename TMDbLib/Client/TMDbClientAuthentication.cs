using System;
using System.Threading.Tasks;
using System.Net;
using TMDbLib.Objects.Authentication;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<GuestSession> AuthenticationCreateGuestSessionAsync()
        {
            RestRequest request = _client.Create("authentication/guest_session/new");
            //{
            //    DateFormat = "yyyy-MM-dd HH:mm:ss UTC"
            //};

            RestResponse<GuestSession> response = await request.ExecuteGet<GuestSession>().ConfigureAwait(false);

            return response;
        }

        public async Task<UserSession> AuthenticationGetUserSessionAsync(string initialRequestToken)
        {
            RestRequest request = _client.Create("authentication/session/new");
            request.AddParameter("request_token", initialRequestToken);

            RestResponse<UserSession> response = await request.ExecuteGet<UserSession>().ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            return response;
        }

        /// <summary>
        /// Conveniance method combining 'AuthenticationRequestAutenticationTokenAsync', 'AuthenticationValidateUserTokenAsync' and 'AuthenticationGetUserSessionAsync'
        /// </summary>
        /// <param name="username">A valid TMDb username</param>
        /// <param name="password">The passoword for the provided login</param>
        public async Task<UserSession> AuthenticationGetUserSessionAsync(string username, string password)
        {
            Token token = await AuthenticationRequestAutenticationTokenAsync().ConfigureAwait(false);
            await AuthenticationValidateUserTokenAsync(token.RequestToken, username, password).ConfigureAwait(false);
            return await AuthenticationGetUserSessionAsync(token.RequestToken).ConfigureAwait(false);
        }

        public async Task<Token> AuthenticationRequestAutenticationTokenAsync()
        {
            RestRequest request = _client.Create("authentication/token/new");

            RestResponse<Token> response = await request.ExecuteGet<Token>().ConfigureAwait(false);
            Token token = response;

            token.AuthenticationCallback = response.GetHeader("Authentication-Callback");

            return token;
        }

        public async Task AuthenticationValidateUserTokenAsync(string initialRequestToken, string username, string password)
        {
            RestRequest request = _client.Create("authentication/token/validate_with_login");
            request.AddParameter("request_token", initialRequestToken);
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            RestResponse response;
            try
            {
                response = await request.ExecuteGet().ConfigureAwait(false);
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided user credentials are invalid.");
            }
        }
    }
}
