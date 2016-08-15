using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Genres;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<SearchContainerWithId<SearchMovie>> GetGenreMoviesAsync(int genreId, int page = 0, bool? includeAllMovies = null)
        {
            return await GetGenreMoviesAsync(genreId, DefaultLanguage, page, includeAllMovies).ConfigureAwait(false);
        }

        public async Task<SearchContainerWithId<SearchMovie>> GetGenreMoviesAsync(int genreId, string language, int page = 0, bool? includeAllMovies = null)
        {
            RestRequest req = _client.Create("genre/{genreId}/movies");
            req.AddUrlSegment("genreId", genreId.ToString());

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (includeAllMovies.HasValue)
                req.AddParameter("include_all_movies", includeAllMovies.Value ? "true" : "false");

            RestResponse<SearchContainerWithId<SearchMovie>> resp = await req.ExecuteGet<SearchContainerWithId<SearchMovie>>().ConfigureAwait(false);

            return resp;
        }

        public async Task<List<Genre>> GetMovieGenresAsync()
        {
            return await GetMovieGenresAsync(DefaultLanguage).ConfigureAwait(false);
        }

        public async Task<List<Genre>> GetMovieGenresAsync(string language)
        {
            RestRequest req = _client.Create("genre/movie/list");

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            RestResponse<GenreContainer> resp = await req.ExecuteGet<GenreContainer>().ConfigureAwait(false);

            return (await resp.GetDataObject().ConfigureAwait(false)).Genres;
        }

        public async Task<List<Genre>> GetTvGenresAsync()
        {
            return await GetTvGenresAsync(DefaultLanguage).ConfigureAwait(false);
        }

        public async Task<List<Genre>> GetTvGenresAsync(string language)
        {
            RestRequest req = _client.Create("genre/tv/list");

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            RestResponse<GenreContainer> resp = await req.ExecuteGet<GenreContainer>().ConfigureAwait(false);

            return (await resp.GetDataObject().ConfigureAwait(false)).Genres;
        }
    }
}