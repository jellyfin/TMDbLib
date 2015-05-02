using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Genres;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<List<Genre>> GetGenres()
        {
            return await GetGenres(DefaultLanguage);
        }

        public async Task<List<Genre>> GetGenres(string language)
        {
            RestRequest req = new RestRequest("genre/list");

            if (language != null)
                req.AddParameter("language", language);

            IRestResponse<GenreContainer> resp =await  _client.ExecuteGetTaskAsync<GenreContainer>(req);

            if (resp.Data == null)
                return null;

            return resp.Data.Genres;
        }

        public async Task<SearchContainerWithId<MovieResult>> GetGenreMovies(int genreId, int page = 0, bool? includeAllMovies = null)
        {
            return await GetGenreMovies(genreId, DefaultLanguage, page, includeAllMovies);
        }

        public async Task<SearchContainerWithId<MovieResult>> GetGenreMovies(int genreId, string language, int page = 0, bool? includeAllMovies = null)
        {
            RestRequest req = new RestRequest("genre/{genreId}/movies");
            req.AddUrlSegment("genreId", genreId.ToString());

            if (language != null)
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page);
            if (includeAllMovies.HasValue)
                req.AddParameter("include_all_movies", includeAllMovies.Value ? "true" : "false");

            IRestResponse<SearchContainerWithId<MovieResult>> resp = await _client.ExecuteGetTaskAsync<SearchContainerWithId<MovieResult>>(req);

            return resp.Data;
        }
    }
}