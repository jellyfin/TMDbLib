using System.Collections.Generic;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Genres;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public List<Genre> GetGenres()
        {
            return GetGenres(DefaultLanguage);
        }

        public List<Genre> GetGenres(string language)
        {
            RestQueryBuilder req = new RestQueryBuilder("genre/list");

            if (language != null)
                req.AddParameter("language", language);

            ResponseContainer<GenreContainer> resp = _client.Get<GenreContainer>(req);

            if (resp.Data == null)
                return null;

            return resp.Data.Genres;
        }

        public SearchContainerWithId<MovieResult> GetGenreMovies(int genreId, int page = 0, bool? includeAllMovies = null)
        {
            return GetGenreMovies(genreId, DefaultLanguage, page, includeAllMovies);
        }

        public SearchContainerWithId<MovieResult> GetGenreMovies(int genreId, string language, int page = 0, bool? includeAllMovies = null)
        {
            RestQueryBuilder req = new RestQueryBuilder("genre/{genreId}/movies");
            req.AddUrlSegment("genreId", genreId.ToString());

            if (language != null)
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());

            if (includeAllMovies.HasValue)
                req.AddParameter("include_all_movies", includeAllMovies.Value ? "true" : "false");

            ResponseContainer<SearchContainerWithId<MovieResult>> resp = _client.Get<SearchContainerWithId<MovieResult>>(req);

            return resp.Data;
        }
    }
}