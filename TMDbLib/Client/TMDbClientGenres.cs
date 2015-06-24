using System.Collections.Generic;
using System.Threading.Tasks;
﻿using System;
﻿using RestSharp;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Genres;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        // TODO: This method no longer appears in Apiary
        [Obsolete]
        public async Task<List<Genre>> GetGenres()
        {
            return await GetGenres(DefaultLanguage);
        }
        
        // TODO: This method no longer appears in Apiary
        [Obsolete]
        public async Task<List<Genre>> GetGenres(string language)
        {
            RestRequest req = new RestRequest("genre/list");

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            IRestResponse<GenreContainer> resp = await _client.ExecuteGetTaskAsync<GenreContainer>(req).ConfigureAwait(false);

            if (resp.Data == null)
                return null;

            return resp.Data.Genres;
        }

        public List<Genre> GetMovieGenres()
        {
            return GetMovieGenres(DefaultLanguage);
        }

        public List<Genre> GetMovieGenres(string language)
        {
            RestRequest req = new RestRequest("genre/movie/list");

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            IRestResponse<GenreContainer> resp = _client.Get<GenreContainer>(req);

            if (resp.Data == null)
                return null;

            return resp.Data.Genres;
        }

        public List<Genre> GetTvGenres()
        {
            return GetTvGenres(DefaultLanguage);
        }

        public List<Genre> GetTvGenres(string language)
        {
            RestRequest req = new RestRequest("genre/tv/list");

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            IRestResponse<GenreContainer> resp = _client.Get<GenreContainer>(req);

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

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page);
            if (includeAllMovies.HasValue)
                req.AddParameter("include_all_movies", includeAllMovies.Value ? "true" : "false");

            IRestResponse<SearchContainerWithId<MovieResult>> resp = await _client.ExecuteGetTaskAsync<SearchContainerWithId<MovieResult>>(req).ConfigureAwait(false);

            return resp.Data;
        }
    }
}