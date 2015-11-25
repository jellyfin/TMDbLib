using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Genres;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<List<Genre>> GetMovieGenres()
        {
            return await GetMovieGenres(DefaultLanguage);
        }

        public async Task<List<Genre>> GetMovieGenres(string language)
        {
            TmdbRestRequest req = _client2.Create("genre/movie/list");

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            TmdbRestResponse<GenreContainer> resp = await req.ExecuteGetTaskAsync<GenreContainer>();

            return (await resp.GetDataObject()).Genres;
        }

        public async Task<List<Genre>> GetTvGenres()
        {
            return await GetTvGenres(DefaultLanguage);
        }

        public async Task<List<Genre>> GetTvGenres(string language)
        {
            TmdbRestRequest req = _client2.Create("genre/tv/list");

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            TmdbRestResponse<GenreContainer> resp = await req.ExecuteGetTaskAsync<GenreContainer>();

            return (await resp.GetDataObject()).Genres;
        }

        public async Task<SearchContainerWithId<MovieResult>> GetGenreMovies(int genreId, int page = 0, bool? includeAllMovies = null)
        {
            return await GetGenreMovies(genreId, DefaultLanguage, page, includeAllMovies);
        }

        public async Task<SearchContainerWithId<MovieResult>> GetGenreMovies(int genreId, string language, int page = 0, bool? includeAllMovies = null)
        {
            TmdbRestRequest req = _client2.Create("genre/{genreId}/movies");
            req.AddUrlSegment("genreId", genreId.ToString());

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (includeAllMovies.HasValue)
                req.AddParameter("include_all_movies", includeAllMovies.Value ? "true" : "false");

            TmdbRestResponse<SearchContainerWithId<MovieResult>> resp = await req.ExecuteGetTaskAsync<SearchContainerWithId<MovieResult>>().ConfigureAwait(false);

            return resp;
        }
    }
}