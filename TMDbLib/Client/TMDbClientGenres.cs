using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Genres;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Retrieves movies for a specific genre.
    /// </summary>
    /// <param name="genreId">The TMDb ID of the genre.</param>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="includeAllMovies">Whether to include all movies or only those with posters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A search container with movies in the specified genre.</returns>
    [Obsolete("GetGenreMovies is deprecated, use DiscoverMovies instead")]
    public async Task<SearchContainerWithId<SearchMovie>> GetGenreMoviesAsync(int genreId, int page = 0, bool? includeAllMovies = null, CancellationToken cancellationToken = default)
    {
        return await GetGenreMoviesAsync(genreId, DefaultLanguage, page, includeAllMovies, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves movies for a specific genre with language option.
    /// </summary>
    /// <param name="genreId">The TMDb ID of the genre.</param>
    /// <param name="language">The ISO 639-1 language code for the movie text. Defaults to the client's DefaultLanguage.</param>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="includeAllMovies">Whether to include all movies or only those with posters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A search container with movies in the specified genre.</returns>
    [Obsolete("GetGenreMovies is deprecated, use DiscoverMovies instead")]
    public async Task<SearchContainerWithId<SearchMovie>> GetGenreMoviesAsync(int genreId, string language, int page = 0, bool? includeAllMovies = null, CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("genre/{genreId}/movies");
        req.AddUrlSegment("genreId", genreId.ToString(CultureInfo.InvariantCulture));

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (includeAllMovies.HasValue)
        {
            req.AddParameter("include_all_movies", includeAllMovies.Value ? "true" : "false");
        }

        SearchContainerWithId<SearchMovie> resp = await req.GetOfT<SearchContainerWithId<SearchMovie>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves a list of all movie genres available on TMDb.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of movie genres with IDs and names.</returns>
    public async Task<List<Genre>> GetMovieGenresAsync(CancellationToken cancellationToken = default)
    {
        return await GetMovieGenresAsync(DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a list of all movie genres available on TMDb with language option.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code for genre names. Defaults to the client's DefaultLanguage.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of movie genres with IDs and names.</returns>
    public async Task<List<Genre>> GetMovieGenresAsync(string language, CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("genre/movie/list");

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        using RestResponse<GenreContainer> resp = await req.Get<GenreContainer>(cancellationToken).ConfigureAwait(false);

        return (await resp.GetDataObject().ConfigureAwait(false)).Genres;
    }

    /// <summary>
    /// Retrieves a list of all TV show genres available on TMDb.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of TV genres with IDs and names.</returns>
    public async Task<List<Genre>> GetTvGenresAsync(CancellationToken cancellationToken = default)
    {
        return await GetTvGenresAsync(DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a list of all TV show genres available on TMDb with language option.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code for genre names. Defaults to the client's DefaultLanguage.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of TV genres with IDs and names.</returns>
    public async Task<List<Genre>> GetTvGenresAsync(string language, CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("genre/tv/list");

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        using RestResponse<GenreContainer> resp = await req.Get<GenreContainer>(cancellationToken).ConfigureAwait(false);

        return (await resp.GetDataObject().ConfigureAwait(false)).Genres;
    }
}
