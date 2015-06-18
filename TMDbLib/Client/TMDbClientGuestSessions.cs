using RestSharp;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public SearchContainer<Movie> GetGuestSessionRatedMovies(int page = 0)
        {
            return GetGuestSessionRatedMovies(DefaultLanguage, page);
        }

        public SearchContainer<Movie> GetGuestSessionRatedMovies(string language, int page = 0)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest request = new RestRequest("guest_session/{guest_session_id}/rated_movies") { RequestFormat = DataFormat.Json };

            if (page > 0)
                request.AddParameter("page", page, ParameterType.QueryString);

            if (!string.IsNullOrEmpty(language))
                request.AddParameter("language", language, ParameterType.QueryString);

            // TODO: Can we use user session for this?
            //if (SessionType == SessionType.UserSession)
            //    request.AddParameter("session_id", SessionId, ParameterType.QueryString);
            //else
            request.AddUrlSegment("guest_session_id", SessionId);


            IRestResponse<SearchContainer<Movie>> resp = _client.Post<SearchContainer<Movie>>(request);

            return resp.Data;
        }
    }
}
