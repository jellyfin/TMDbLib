using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<SearchContainer<TvEpisodeWithRating>> GetGuestSessionRatedTvEpisodes(int page = 0)
        {
            return await GetGuestSessionRatedTvEpisodes(DefaultLanguage, page);
        }

        public async Task<SearchContainer<TvEpisodeWithRating>> GetGuestSessionRatedTvEpisodes(string language, int page = 0)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest request = new RestRequest("guest_session/{guest_session_id}/rated/tv/episodes") { RequestFormat = DataFormat.Json };

            if (page > 0)
                request.AddParameter("page", page, ParameterType.QueryString);

            if (!string.IsNullOrEmpty(language))
                request.AddParameter("language", language, ParameterType.QueryString);

            AddSessionId(request, SessionType.GuestSession, ParameterType.UrlSegment);

            IRestResponse<SearchContainer<TvEpisodeWithRating>> resp = await _client.ExecuteGetTaskAsync<SearchContainer<TvEpisodeWithRating>>(request).ConfigureAwait(false);

            return resp.Data;
        }

        public async Task<SearchContainer<TvShowWithRating>> GetGuestSessionRatedTv(int page = 0)
        {
            return await GetGuestSessionRatedTv(DefaultLanguage, page);
        }

        public async Task<SearchContainer<TvShowWithRating>> GetGuestSessionRatedTv(string language, int page = 0)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest request = new RestRequest("guest_session/{guest_session_id}/rated/tv") { RequestFormat = DataFormat.Json };

            if (page > 0)
                request.AddParameter("page", page, ParameterType.QueryString);

            if (!string.IsNullOrEmpty(language))
                request.AddParameter("language", language, ParameterType.QueryString);
            
            AddSessionId(request, SessionType.GuestSession, ParameterType.UrlSegment);

            IRestResponse<SearchContainer<TvShowWithRating>> resp = await _client.ExecuteGetTaskAsync<SearchContainer<TvShowWithRating>>(request).ConfigureAwait(false);

            return resp.Data;
        }

        public async Task<SearchContainer<MovieWithRating>> GetGuestSessionRatedMovies(int page = 0)
        {
            return await GetGuestSessionRatedMovies(DefaultLanguage, page);
        }

        public async Task<SearchContainer<MovieWithRating>> GetGuestSessionRatedMovies(string language, int page = 0)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest request = new RestRequest("guest_session/{guest_session_id}/rated/movies") { RequestFormat = DataFormat.Json };

            if (page > 0)
                request.AddParameter("page", page, ParameterType.QueryString);

            if (!string.IsNullOrEmpty(language))
                request.AddParameter("language", language, ParameterType.QueryString);
            
            AddSessionId(request, SessionType.GuestSession, ParameterType.UrlSegment);

            IRestResponse<SearchContainer<MovieWithRating>> resp = await _client.ExecuteGetTaskAsync<SearchContainer<MovieWithRating>>(request).ConfigureAwait(false);

            return resp.Data;
        }
    }
}
