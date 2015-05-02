using System;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.Authentication;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Token> AuthenticationRequestAutenticationToken()
        {
            RestRequest request = new RestRequest("authentication/token/new")
            {
                DateFormat = "yyyy-MM-dd HH:mm:ss UTC"
            };

            IRestResponse<Token> response = await _client.ExecuteGetTaskAsync<Token>(request);
            Token token = response.Data;

            token.AuthenticationCallback = response.Headers.First(h => h.Name.Equals("Authentication-Callback")).Value.ToString();

            return token;
        }

        public async void AuthenticationValidateUserToken(string initialRequestToken, string username, string password)
        {
            RestRequest request = new RestRequest("authentication/token/validate_with_login");
            request.AddParameter("request_token", initialRequestToken);
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            IRestResponse response = await _client.ExecuteGetTaskAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided user credentials are invalid.");
            }
        }

        public async Task<UserSession> AuthenticationGetUserSession(string initialRequestToken)
        {
            RestRequest request = new RestRequest("authentication/session/new");
            request.AddParameter("request_token", initialRequestToken);

            IRestResponse<UserSession> response = await _client.ExecuteGetTaskAsync<UserSession>(request);

            return response.Data;
        }

        /// <summary>
        /// Conveniance method combining 'AuthenticationRequestAutenticationToken', 'AuthenticationValidateUserToken' and 'AuthenticationGetUserSession'
        /// </summary>
        /// <param name="username">A valid TMDb username</param>
        /// <param name="password">The passoword for the provided login</param>
        public async Task<UserSession> AuthenticationGetUserSession(string username, string password)
        {
            Token token = await AuthenticationRequestAutenticationToken();
            AuthenticationValidateUserToken(token.RequestToken, username, password);
            return await AuthenticationGetUserSession(token.RequestToken);
        }

        public async Task<GuestSession> AuthenticationCreateGuestSession()
        {
            RestRequest request = new RestRequest("authentication/guest_session/new")
            {
                DateFormat = "yyyy-MM-dd HH:mm:ss UTC"
            };

            IRestResponse<GuestSession> response = await _client.ExecuteGetTaskAsync<GuestSession>(request);

            return response.Data;
        }
    }
}
