using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Genres;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<SearchContainerWithId<SearchMovie>> GetGenreMoviesAsync(int genreId, int page = 0, bool? includeAllMovies = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetGenreMoviesAsync(genreId, DefaultLanguage, page, includeAllMovies, cancellationToken).ConfigureAwait(false);
        }

        public async Task<SearchContainerWithId<SearchMovie>> GetGenreMoviesAsync(int genreId, string language, int page = 0, bool? includeAllMovies = null, CancellationToken cancellationToken = default(CancellationToken))
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

            RestResponse<SearchContainerWithId<SearchMovie>> resp = await req.ExecuteGet<SearchContainerWithId<SearchMovie>>(cancellationToken).ConfigureAwait(false);

            return resp;
        }

        public async Task<List<Genre>> GetMovieGenresAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetMovieGenresAsync(DefaultLanguage, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Genre>> GetMovieGenresAsync(string language, CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("genre/movie/list");

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            RestResponse<GenreContainer> resp = await req.ExecuteGet<GenreContainer>(cancellationToken).ConfigureAwait(false);

            return (await resp.GetDataObject().ConfigureAwait(false)).Genres;
        }

        public async Task<List<Genre>> GetTvGenresAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetTvGenresAsync(DefaultLanguage, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Genre>> GetTvGenresAsync(string language, CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("genre/tv/list");

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            RestResponse<GenreContainer> resp = await req.ExecuteGet<GenreContainer>(cancellationToken).ConfigureAwait(false);

            return (await resp.GetDataObject().ConfigureAwait(false)).Genres;
        }
    }
}