using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;
using TMDbLib.Rest;

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

            TmdbRestRequest request = _client2.Create("guest_session/{guest_session_id}/rated/tv/episodes");

            if (page > 0)
                request.AddParameter("page", page.ToString());

            if (!string.IsNullOrEmpty(language))
                request.AddParameter("language", language);

            AddSessionId(request, SessionType.GuestSession, TmdbParameterType.UrlSegment);

            TmdbRestResponse<SearchContainer<TvEpisodeWithRating>> resp = await request.ExecuteGet<SearchContainer<TvEpisodeWithRating>>().ConfigureAwait(false);

            return resp;
        }

        public async Task<SearchContainer<TvShowWithRating>> GetGuestSessionRatedTv(int page = 0)
        {
            return await GetGuestSessionRatedTv(DefaultLanguage, page);
        }

        public async Task<SearchContainer<TvShowWithRating>> GetGuestSessionRatedTv(string language, int page = 0)
        {
            RequireSessionId(SessionType.GuestSession);

            TmdbRestRequest request = _client2.Create("guest_session/{guest_session_id}/rated/tv");

            if (page > 0)
                request.AddParameter("page", page.ToString());

            if (!string.IsNullOrEmpty(language))
                request.AddParameter("language", language);

            AddSessionId(request, SessionType.GuestSession, TmdbParameterType.UrlSegment);

            TmdbRestResponse<SearchContainer<TvShowWithRating>> resp = await request.ExecuteGet<SearchContainer<TvShowWithRating>>().ConfigureAwait(false);

            return resp;
        }

        public async Task<SearchContainer<MovieWithRating>> GetGuestSessionRatedMovies(int page = 0)
        {
            return await GetGuestSessionRatedMovies(DefaultLanguage, page);
        }

        public async Task<SearchContainer<MovieWithRating>> GetGuestSessionRatedMovies(string language, int page = 0)
        {
            RequireSessionId(SessionType.GuestSession);

            TmdbRestRequest request = _client2.Create("guest_session/{guest_session_id}/rated/movies");

            if (page > 0)
                request.AddParameter("page", page.ToString());

            if (!string.IsNullOrEmpty(language))
                request.AddParameter("language", language);

            AddSessionId(request, SessionType.GuestSession, TmdbParameterType.UrlSegment);

            TmdbRestResponse<SearchContainer<MovieWithRating>> resp = await request.ExecuteGet<SearchContainer<MovieWithRating>>().ConfigureAwait(false);

            return resp;
        }
    }
}
