using System.Linq;
using TMDbLib.Objects.Authentication;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Token AuthenticationRequestAutenticationToken()
        {
            RestQueryBuilder request = new RestQueryBuilder("authentication/token/new");
            //{
            //    DateFormat = "yyyy-MM-dd HH:mm:ss UTC"
            //};

            ResponseContainer<Token> response = _client.Get<Token>(request);
            Token token = response.Data;

            token.AuthenticationCallback = response.Headers.First(h => h.Key.Equals("Authentication-Callback")).Value.ToString();

            return token;
        }

        public UserSession AuthenticationGetUserSession(string initialRequestToken)
        {
            RestQueryBuilder request = new RestQueryBuilder("authentication/session/new");
            request.AddParameter("request_token", initialRequestToken);

            ResponseContainer<UserSession> response = _client.Get<UserSession>(request);

            return response.Data;
        }

        public GuestSession AuthenticationCreateGuestSession()
        {
            RestQueryBuilder request = new RestQueryBuilder("authentication/guest_session/new");
            //{
            //    DateFormat = "yyyy-MM-dd HH:mm:ss UTC"
            //};

            ResponseContainer<GuestSession> response = _client.Get<GuestSession>(request);

            return response.Data;
        }
    }
}
